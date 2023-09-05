#nullable enable
using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Wordsmith.DataAccess.Db;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Models;
using Wordsmith.Models.Exceptions;
using Wordsmith.Models.MessageObjects;
using Wordsmith.Models.RequestObjects;
using Wordsmith.Models.SearchObjects;
using Wordsmith.Utils;
using Wordsmith.Utils.LoginClient;
using Wordsmith.Utils.RabbitMQ;

namespace Wordsmith.DataAccess.Services;

public class UserService : WriteService<UserDto, User, SearchObject, UserInsertRequest, UserUpdateRequest>, IUserService
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

    protected override async Task BeforeInsert(User entity, UserInsertRequest insert)
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

    protected override Task AfterInsert(User entity, UserInsertRequest insert)
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

        Logger.LogDebug($"Got access token {tokens.AccessToken}");

        return new OkObjectResult(tokens);
    }

    public async Task<ActionResult<UserDto>> UpdateProfile(string? userIdStr, UserUpdateRequest request)
    {
        if (!int.TryParse(userIdStr, out var userId))
        {
            throw new AppException("User could not be parsed");
        }

        var entity = await Context.Users.FindAsync(userId);

        if (entity == null)
        {
            throw new AppException("User does not exist");
        }

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

    public async Task<ActionResult<UserLoginDto>> Refresh(string? bearerToken, string client)
    {
        var refreshToken = bearerToken?.Replace("Bearer", "").Trim();

        if (refreshToken == null) throw new AppException("No refresh token was passed");

        var clientSecret = "";
        var clientId = "";

        switch (client)
        {
            case "user":
                clientSecret = _configuration["IdentityServer:Secrets:User"];
                clientId = "user.client";
                break;
            case "admin":
                clientSecret = _configuration["IdentityServer:Secrets:Admin"];
                clientId = "admin.client";
                break;
        }

        if (string.IsNullOrEmpty(clientSecret))
        {
            throw new Exception("IdentityServer client secret has not been configured!");
        }

        var tokens = await _loginClient.RequestAccess(refreshToken, clientId, clientSecret);

        Logger.LogDebug($"Got access token {tokens.AccessToken}");

        return new OkObjectResult(tokens);
    }

    public async Task<ActionResult> ChangeAccess(string adminIdStr, int userId, UserChangeAccessRequest changeAccess)
    {
        var user = await Context.Users.FindAsync(userId);

        if (user == null)
        {
            throw new AppException("The user does not exist!");
        }

        if (!int.TryParse(adminIdStr, out var adminId))
        {
            throw new AppException("User id was null or could not be parsed");
        }

        var admin = await Context.Users.FindAsync(adminId);

        if (admin == null)
        {
            throw new AppException("The admin does not exist!");
        }

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
}