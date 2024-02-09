#nullable enable
using System.Security.Claims;
using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Wordsmith.DataAccess.Db;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.Exceptions;
using Wordsmith.Models.MessageObjects;
using Wordsmith.Models.RequestObjects;
using Wordsmith.Models.SearchObjects;
using Wordsmith.Utils;
using Wordsmith.Utils.LoginClient;
using Wordsmith.Utils.RabbitMQ;

namespace Wordsmith.DataAccess.Services.User;

public class UserService : WriteService<UserDto, Db.Entities.User, SearchObject, UserInsertRequest, UserUpdateRequest>, IUserService
{
    private readonly IMessageProducer _messageProducer;
    private readonly IMessageListener _messageListener;
    private readonly ILoginClient _loginClient;
    private readonly IConfiguration _configuration;

    private const int ReplyGracePeriod = 10;

    public UserService(DatabaseContext databaseContext, IMapper mapper, IMessageProducer messageProducer,
        ILoginClient loginClient, IConfiguration configuration, IMessageListener messageListener)
        : base(databaseContext, mapper)
    {
        _messageProducer = messageProducer;
        _loginClient = loginClient;
        _configuration = configuration;
        _messageListener = messageListener;
    }

    protected override async Task BeforeInsert(Db.Entities.User entity, UserInsertRequest insert)
    {
        if (await AlreadyExists(insert))
        {
            throw new AppException("User with the provided username or email already exists");
        }

        entity.RegistrationDate = DateTime.UtcNow;
        entity.Role = "user";

        if (insert.ProfileImage?.EncodedImage == null) return;

        var savePath = Path.Combine("images", "users",
            $"{entity.Username}_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}.{insert.ProfileImage.Format}");
        var result = ImageHelper.SaveFromBase64(insert.ProfileImage.EncodedImage, insert.ProfileImage.Format, savePath);

        var newImageEntity = new Image()
        {
            Path = result.Path,
            Format = result.Format,
            Size = result.Size
        };

        await Context.Images.AddAsync(newImageEntity);
        entity.ProfileImage = newImageEntity;
    }

    protected override Task AfterInsert(Db.Entities.User entity, UserInsertRequest insert)
    {
        _messageProducer.SendMessage("user_insertion", new RegisterUserMessage()
        {
            Id = entity.Id,
            Username = entity.Username,
            Email = entity.Email,
            Password = insert.Password
        });

        return Task.CompletedTask;
    }

    public async Task<ActionResult<UserLoginDto>> Login(UserLoginRequest login)
    {
        var entity = await Context.Users.FirstOrDefaultAsync(user => user.Username == login.Username);

        if (entity == null)
        {
            throw new AppException("User does not exist");
        }

        var clientSecret = "";
        var clientId = "";
        var scopes = "";

        if (string.Equals(entity.Role, "admin", StringComparison.OrdinalIgnoreCase))
        {
            clientSecret = _configuration["IdentityServer:Secrets:Admin"];
            clientId = "admin.client";
            scopes = "offline_access wordsmith_api.full_access";
        }
        else if (string.Equals(entity.Role, "user", StringComparison.OrdinalIgnoreCase))
        {
            clientSecret = _configuration["IdentityServer:Secrets:User"];
            clientId = "user.client";
            scopes = "offline_access wordsmith_api.read wordsmith_api.write";
        }

        if (string.IsNullOrEmpty(clientSecret))
        {
            throw new Exception("IdentityServer client secret has not been configured!");
        }

        var tokens = await _loginClient.RequestAccess(login, clientId, clientSecret, scopes);
        tokens.User = Mapper.Map<UserDto>(entity);
        
        Logger.LogDebug($"Created login session for user with id {entity.Id}");

        return new OkObjectResult(tokens);
    }

    public async Task<ActionResult<UserDto>> UpdateProfile(UserUpdateRequest request, IEnumerable<Claim> userClaims)
    {
        var entity = await GetUserFromClaims(userClaims);

        Mapper.Map(request, entity);

        var id = _messageProducer.SendMessage("user_update", new UpdateUserMessage()
        {
            Id = entity.Id,
            Username = entity.Username,
            Email = entity.Email,
            Password = request.Password,
            OldPassword = request.OldPassword
        });

        var autoResetEvent = new AutoResetEvent(false);
        var response = await WaitForReply(autoResetEvent, "user_update_replies", id);

        Logger.LogDebug($"Got reply with id {id} from IdentityServer when updating user information");

        if (!response.Succeeded)
        {
            throw new AppException($"Could not change access for user {entity.Username}!",
                new Dictionary<string, object>()
                {
                    { "reason", response.Errors }
                });
        }

        await Context.SaveChangesAsync();
        
        return new OkObjectResult(Mapper.Map<UserDto>(entity));
    }

    public async Task<ActionResult<QueryResult<ImageDto>>> GetProfileImage(IEnumerable<Claim> userClaims)
    {
        var entity = await GetUserFromClaims(userClaims);

        await Context.Entry(entity).Reference(e => e.ProfileImage).LoadAsync(); // ProfileImage must be loaded in before 
        
        var result = new QueryResult<ImageDto>()
        {
            Result = new List<ImageDto>()
        };

        if (entity.ProfileImage != null)
        {
            result.Result.Add(Mapper.Map<ImageDto>(entity.ProfileImage));
        }

        return result;
    }

    public async Task<ActionResult<ImageDto>> UpdateProfileImage(ImageInsertRequest update, IEnumerable<Claim> userClaims)
    {
        var entity = await GetUserFromClaims(userClaims);
        
        await Context.Entry(entity).Reference(e => e.ProfileImage).LoadAsync(); // ProfileImage must be loaded in before 

        // This is kept as a call prior to the creation of the new image in case deletion of the existing image fails
        // In that case it's better to immediately end execution here when DeleteImage returns an exception
        if (entity.ProfileImage != null)
        {
            ImageHelper.DeleteImage(entity.ProfileImage.Path);
        }
        
        ImageDto updatedImage;
        var savePath = Path.Combine("images", "users",
            $"{entity.Username}_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}.{update.Format}");
        var result = ImageHelper.SaveFromBase64(update.EncodedImage, update.Format, savePath);
        
        if (entity.ProfileImage != null)
        {
            entity.ProfileImage.Format = result.Format;
            entity.ProfileImage.Path = result.Path;
            entity.ProfileImage.Size = result.Size;
            updatedImage = Mapper.Map<ImageDto>(entity.ProfileImage);
        }
        else
        {
            var newImageEntity = new Image()
            {
                Path = result.Path,
                Format = result.Format,
                Size = result.Size
            };
            
            await Context.Images.AddAsync(newImageEntity);
            entity.ProfileImage = newImageEntity;
            updatedImage = Mapper.Map<ImageDto>(entity.ProfileImage);
        }
        
        await Context.SaveChangesAsync();

        return updatedImage;
    }

    public async Task<ActionResult<QueryResult<UserLoginDto>>> Refresh(string? bearerToken, int id)
    {
        var refreshToken = bearerToken?.Replace("Bearer", "").Trim();

        if (refreshToken == null) throw new AppException("No refresh token was passed");

        var entity = await Context.Users.FindAsync(id);

        if (entity == null) throw new AppException("User with the id passed was not found");
        
        var clientSecret = "";
        var clientId = "";

        if (string.Equals(entity.Role, "admin", StringComparison.OrdinalIgnoreCase))
        {
            clientSecret = _configuration["IdentityServer:Secrets:Admin"];
            clientId = "admin.client";
        }
        else if (string.Equals(entity.Role, "user", StringComparison.OrdinalIgnoreCase))
        {
            clientSecret = _configuration["IdentityServer:Secrets:User"];
            clientId = "user.client";
        }

        if (string.IsNullOrEmpty(clientSecret))
        {
            throw new Exception("IdentityServer client secret has not been configured!");
        }

        var tokens = await _loginClient.RequestAccess(refreshToken, clientId, clientSecret);
        tokens.User = Mapper.Map<UserDto>(entity);

        Logger.LogDebug($"Refreshed login session for user with id ${entity.Id}");

        return new OkObjectResult(new QueryResult<UserLoginDto>()
        {
            Result = new List<UserLoginDto>() { tokens }
        });
    }

    public async Task<ActionResult<QueryResult<UserLoginDto>>> VerifyLogin(string? bearerToken, IEnumerable<Claim> userClaims)
    {
        var accessToken = bearerToken?.Replace("Bearer", "").Trim();

        if (accessToken == null) throw new AppException("No refresh token was passed");

        var entity = await GetUserFromClaims(userClaims);
        var clientSecret = "";
        var clientId = "";

        if (string.Equals(entity.Role, "admin", StringComparison.OrdinalIgnoreCase))
        {
            clientSecret = _configuration["IdentityServer:Secrets:Admin"];
            clientId = "admin.client";
        }
        else if (string.Equals(entity.Role, "user", StringComparison.OrdinalIgnoreCase))
        {
            clientSecret = _configuration["IdentityServer:Secrets:User"];
            clientId = "user.client";
        }
        
        if (string.IsNullOrEmpty(clientSecret))
        {
            throw new Exception("IdentityServer client secret has not been configured!");
        }

        var userLogin = await _loginClient.VerifyAccess(accessToken, clientId, clientSecret);
        userLogin.User = Mapper.Map<UserDto>(entity);
        
        Logger.LogDebug($"Verified access token validity for user with id {entity.Id}");

        return new OkObjectResult(new QueryResult<UserLoginDto>()
        {
            Result = new List<UserLoginDto>() { userLogin }
        });
    }

    public async Task<ActionResult> ChangeAccess(int userId, UserChangeAccessRequest changeAccess, IEnumerable<Claim> userClaims)
    {
        var user = await Context.Users.FindAsync(userId);

        if (user == null)
        {
            throw new AppException("The user does not exist!");
        }

        var admin = await GetUserFromClaims(userClaims);

        var alreadyRemovedAccess = await Context.UserBans.AnyAsync(userBan =>
            userBan.UserId == userId &&
            (userBan.ExpiryDate == null || userBan.ExpiryDate.Value > DateTime.UtcNow));

        // If attempting to persist a change that's already present, just return OK
        if ((alreadyRemovedAccess && !changeAccess.AllowedAccess) ||
            (!alreadyRemovedAccess && changeAccess.AllowedAccess))
        {
            return new OkResult();
        }

        if (!changeAccess.AllowedAccess)
        {
            // Make a record in the database to indicate that the user has been banned
            var newBan = new UserBan()
            {
                Admin = admin,
                User = user,
                BannedDate = DateTime.UtcNow,
                ExpiryDate = changeAccess.ExpiryDate
            };

            await Context.UserBans.AddAsync(newBan);
        }
        else
        {
            // Make sure that the database reflects that the user is unbanned
            var lastRemoval = await Context.UserBans
                .Where(userBan => userBan.UserId == userId)
                .OrderByDescending(userBan => userBan.BannedDate)
                .FirstOrDefaultAsync();

            if (lastRemoval == null) throw new Exception("The ban for this user not found!");
            
            lastRemoval.ExpiryDate = DateTime.UtcNow;
        }

        // Persist the access status over at the IdentityServer
        var id = _messageProducer.SendMessage("user_change_access", new ChangeUserAccessMessage()
        {
            Id = user.Id,
            AllowedAccess = changeAccess.AllowedAccess,
            ExpiryDate = changeAccess.ExpiryDate
        });

        var autoResetEvent = new AutoResetEvent(false);
        var response = await WaitForReply(autoResetEvent, "user_change_access_replies", id);

        Logger.LogDebug($"Got reply with id {id} from IdentityServer when updating user information");

        if (!response.Succeeded)
        {
            throw new AppException($"Could not change access for user {user.Username}!",
                new Dictionary<string, object>()
                {
                    { "reason", response.Errors }
                });
        }

        await Context.SaveChangesAsync();
        return new OkResult();
    }

    private async Task<bool> AlreadyExists(UserInsertRequest insert)
    {
        return await Context.Users.AnyAsync(user => user.Username == insert.Username || user.Email == insert.Email);
    }

    private async Task<OperationStatusMessage> WaitForReply(EventWaitHandle autoResetEvent, string queue,
        string correlationId)
    {
        var response = new OperationStatusMessage();

        // Here we listen for the reply of the IdentityServer
        // If the server replies, then we signal the thread to continue execution (with the provided wait handle) and
        // deserialize the reply as a status message
        await _messageListener.Listen(queue, (message, id) =>
        {
            if (correlationId == id)
            {
                response = JsonSerializer.Deserialize<OperationStatusMessage>(message);
                autoResetEvent.Set();
            }

            // Unnecessary, but the delegate requires it for now
            return Task.FromResult(new OperationStatusMessage() { Succeeded = true });
        });

        // If the thread is not signaled within an arbitrary time period (the TimeSpan provided), then we end the request 
        if (!await Task.Run(() => autoResetEvent.WaitOne(TimeSpan.FromSeconds(ReplyGracePeriod))))
        {
            throw new Exception("Request towards the user store timed out");
        }

        return response;
    }

    public async Task<Db.Entities.User> GetUserFromClaims(IEnumerable<Claim> userClaims)
    {
        var refId = userClaims.FirstOrDefault(c => c.Type == "user_ref_id")!.Value; // Should never be null unless tokens are misconfigured
        
        if (!int.TryParse(refId, out var userId))
        {
            throw new Exception($"User with ref id {refId} could not be parsed!");
        }

        var entity = await Context.Users.FindAsync(userId);

        return entity ?? throw new AppException("User passed for auth does not exist!");
    }

    public async Task<QueryResult<UserDto>> GetUserFromClaimsAsDto(IEnumerable<Claim> userClaims)
    {
        var entity = await GetUserFromClaims(userClaims);

        return new QueryResult<UserDto>()
        {
            Result = new List<UserDto>() { Mapper.Map<UserDto>(entity) }
        };
    }
}