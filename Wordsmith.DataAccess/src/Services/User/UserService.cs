#nullable enable
using System.Globalization;
using System.Text.Json;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Wordsmith.DataAccess.Db;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.Enums;
using Wordsmith.Models.Exceptions;
using Wordsmith.Models.MessageObjects;
using Wordsmith.Models.RequestObjects.Image;
using Wordsmith.Models.RequestObjects.Statistics;
using Wordsmith.Models.RequestObjects.User;
using Wordsmith.Models.SearchObjects;
using Wordsmith.Utils;
using Wordsmith.Utils.LoginClient;
using Wordsmith.Utils.RabbitMQ;
using Wordsmith.Utils.StatisticsHelper;

namespace Wordsmith.DataAccess.Services.User;

public class UserService : WriteService<UserDto, Db.Entities.User, UserSearchObject, UserInsertRequest, UserUpdateRequest>, IUserService
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

    protected override async Task BeforeInsert(Db.Entities.User entity, UserInsertRequest insert, int userId)
    {
        insert.Username = Base64Helper.DecodeFromBase64(insert.Username);
        insert.Password = Base64Helper.DecodeFromBase64(insert.Password);
        insert.ConfirmPassword = Base64Helper.DecodeFromBase64(insert.ConfirmPassword);
        insert.Email = Base64Helper.DecodeFromBase64(insert.Email);
        
        await ValidateUsername(insert.Username);
        await ValidateEmail(insert.Email);

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

    protected override Task AfterInsert(Db.Entities.User entity, UserInsertRequest insert, int userId)
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

    protected override IQueryable<Db.Entities.User> AddInclude(IQueryable<Db.Entities.User> query, int userId)
    {
        query = query.Include(e => e.ProfileImage);
        return query;
    }

    protected override IQueryable<Db.Entities.User> AddFilter(IQueryable<Db.Entities.User> query, UserSearchObject search, int userId)
    {
        if (search.Username != null)
        {
            query = query.Where(e => e.Username.Contains(search.Username, StringComparison.OrdinalIgnoreCase));
        }

        return query;
    }

    public async Task<EntityResult<UserLoginDto>> Login(UserLoginRequest login)
    {
        login.Username = Base64Helper.DecodeFromBase64(login.Username);
        login.Password = Base64Helper.DecodeFromBase64(login.Password);
        
        var entity = await Context.Users.Include(e => e.ProfileImage).FirstOrDefaultAsync(user => user.Username == login.Username);

        if (entity == null)
        {
            throw new AppException("Wrong username!");
        }

        var clientId = "";
        var clientSecret = "";
        var scopes = "";

        if (string.Equals(entity.Role, "admin", StringComparison.OrdinalIgnoreCase))
        {
            clientSecret = _configuration["IdentityServer:Secrets:Admin"];
            scopes = "offline_access wordsmith_api.full_access";
            clientId = string.IsNullOrEmpty(login.ClientId) ? "admin.client" : login.ClientId;
        }
        else if (string.Equals(entity.Role, "user", StringComparison.OrdinalIgnoreCase))
        {
            clientSecret = _configuration["IdentityServer:Secrets:User"];
            scopes = "offline_access wordsmith_api.read wordsmith_api.write";
            clientId = string.IsNullOrEmpty(login.ClientId) ? "user.client" : login.ClientId;
        }

        if (string.IsNullOrEmpty(clientSecret))
        {
            throw new Exception("IdentityServer client secret has not been configured!");
        }

        var tokens = await _loginClient.RequestAccess(login, clientId, clientSecret, scopes);
        tokens.User = Mapper.Map<UserDto>(entity);
        
        Logger.LogDebug($"Created login session for user with id {entity.Id}");

        return new EntityResult<UserLoginDto>()
        {
            Message = "Created login session",
            Result = tokens
        };
    }

    public async Task<EntityResult<UserDto>> UpdateProfile(UserUpdateRequest request, int userId)
    {
        var entity = await UserExists(userId);

        await ValidateUpdateProfile(entity, request);

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
            throw new AppException(response.Errors.First(),
                new Dictionary<string, object>()
                {
                    { "reason", response.Errors }
                });
        }

        await Context.SaveChangesAsync();

        return new EntityResult<UserDto>()
        {
            Message = "Updated profile",
            Result = Mapper.Map<UserDto>(entity)
        };
    }

    public async Task<QueryResult<ImageDto>> GetProfileImage(int userId)
    {
        var entity = await UserExists(userId);

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

    public async Task<EntityResult<UserDto>> UpdateProfileImage(ImageInsertRequest update, int userId)
    {
        var entity = await UserExists(userId);
        
        await Context.Entry(entity).Reference(e => e.ProfileImage).LoadAsync(); // ProfileImage must be loaded in before 

        // This is kept as a call prior to the creation of the new image in case deletion of the existing image fails
        // In that case it's better to immediately end execution here when DeleteImage returns an exception
        if (entity.ProfileImage != null)
        {
            ImageHelper.DeleteImage(entity.ProfileImage.Path);
        }
        
        var savePath = Path.Combine("images", "users",
            $"{entity.Username}_{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}.{update.Format}");
        var result = ImageHelper.SaveFromBase64(update.EncodedImage, update.Format, savePath);
        
        if (entity.ProfileImage != null)
        {
            entity.ProfileImage.Format = result.Format;
            entity.ProfileImage.Path = result.Path;
            entity.ProfileImage.Size = result.Size;
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
        }
        
        await Context.SaveChangesAsync();

        return new EntityResult<UserDto>()
        {
            Message = "Updated profile image",
            Result = Mapper.Map<UserDto>(entity),
        };
    }

    public async Task<QueryResult<UserLoginDto>> Refresh(string? bearerToken, int userId)
    {
        var refreshToken = bearerToken?.Replace("Bearer", "").Trim();

        if (refreshToken == null) throw new AppException("No refresh token was passed");

        var entity = await UserExists(userId);
        
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

        return new QueryResult<UserLoginDto>()
        {
            Result = new List<UserLoginDto>() { tokens }
        };
    }

    public async Task<QueryResult<UserLoginDto>> VerifyLogin(string? bearerToken, int userId)
    {
        var accessToken = bearerToken?.Replace("Bearer", "").Trim();

        if (accessToken == null) throw new AppException("No refresh token was passed");

        var entity = await UserExists(userId);
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

        return new QueryResult<UserLoginDto>()
        {
            Result = new List<UserLoginDto>() { userLogin }
        };
    }

    public async Task<EntityResult<UserDto>> ChangeAccess(int userId, UserChangeAccessRequest changeAccess, int adminId)
    {
        await using var transaction = await Context.Database.BeginTransactionAsync();
        
        var user = await UserExists(userId);
        var admin = await UserExists(adminId);

        var alreadyRemovedAccess = await Context.UserBans.AnyAsync(userBan =>
            userBan.UserId == userId &&
            (userBan.ExpiryDate == null || userBan.ExpiryDate.Value > DateTime.UtcNow));

        // If attempting to persist a change that's already present, just return OK
        if ((alreadyRemovedAccess && !changeAccess.AllowedAccess) ||
            (!alreadyRemovedAccess && changeAccess.AllowedAccess))
        {
            return new EntityResult<UserDto>()
            {
                Message = "Access already changed",
                Result = Mapper.Map<UserDto>(user)
            };
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
            user.Status = changeAccess.ExpiryDate.HasValue ? UserStatus.TemporarilyBanned : UserStatus.Banned;
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
            user.Status = UserStatus.Active;
        }

        try
        {
            if (changeAccess.AllowedAccess)
            {
                await UnhideAuthorEbooks(user.Id);
            }
            else
            {
                await HideAuthorEbooks(user.Id);
            }
            
            await Context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw;
        }

        var response = await SendMessageForAccessChange(changeAccess, userId);
        
        if (!response.Succeeded)
        {
            await transaction.RollbackAsync();
            throw new AppException($"Could not change access for user {user.Username}!",
                new Dictionary<string, object>()
                {
                    { "reason", response.Errors }
                });
        }

        await transaction.CommitAsync();
        
        return new EntityResult<UserDto>()
        {
            Message = "Changed access for user",
            Result = Mapper.Map<UserDto>(user)
        };
    }

    public async Task<QueryResult<UserStatisticsDto>> GetUserStatistics(int userId)
    {
        var user = await UserExists(userId);

        var userStatistics = new UserStatisticsDto()
        {
            PublishedBooksCount = await Context.EBooks.CountAsync(e => e.AuthorId == user.Id),
            FavoriteBooksCount = await Context.FavoriteEBooks.CountAsync(e => e.UserId == user.Id)
        };

        return new QueryResult<UserStatisticsDto>()
        {
            Result = new List<UserStatisticsDto>() { userStatistics },
        };
    }
    
    public async Task<QueryResult<UserRegistrationStatisticsDto>> GetRegistrationStatistics(StatisticsRequest request)
    {
        var allMonths = StatisticsHelper.GetAllMonthsInRange(request.StartDate, request.EndDate);
        var registrations = await Context.Users
            .Where(e => e.RegistrationDate.Date >= request.StartDate.Date &&
                        e.RegistrationDate.Date <= request.EndDate.Date)
            .GroupBy(e => new { Month = e.RegistrationDate.Month, Year = e.RegistrationDate.Year })
            .Select(g => new
            {
                Month = g.Key.Month,
                Year = g.Key.Year,
                RegistrationCount = g.Count()
            })
            .ToListAsync();
        
        var result = allMonths.Select(month =>
        {
            var registrationsForMonth = registrations.FirstOrDefault(r => r.Year == month.Year && r.Month == month.Month);
            return new UserRegistrationStatisticsDto
            {
                Month = CultureInfo.InvariantCulture.DateTimeFormat.GetMonthName(month.Month),
                Year = month.Year,
                RegistrationCount = registrationsForMonth?.RegistrationCount ?? 0
            };
        }).ToList();
        
        return new QueryResult<UserRegistrationStatisticsDto>()
        {
            Result = result,
            TotalCount = result.Count
        };
    }
    
    private async Task ValidateUsername(string username)
    {
        if (await Context.Users.AnyAsync(u => u.Username == username))
        {
            throw new AppException("Username is taken!");
        }
    }

    private async Task ValidateEmail(string email)
    {
        if (await Context.Users.AnyAsync(u => u.Email == email))
        {
            throw new AppException("Email is taken!");
        }
    }

    private async Task<Db.Entities.User> UserExists(int userId)
    {
        var user = await Context.Users.Include(e => e.ProfileImage).FirstOrDefaultAsync(e => e.Id == userId);

        if (user == null)
        {
            throw new AppException("User does not exist!");
        }

        return user;
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

    private async Task<OperationStatusMessage> SendMessageForAccessChange(UserChangeAccessRequest changeAccess,
        int userId)
    {
        // Persist the access status over at the IdentityServer
        var id = _messageProducer.SendMessage("user_change_access", new ChangeUserAccessMessage()
        {
            Id = userId,
            AllowedAccess = changeAccess.AllowedAccess,
            ExpiryDate = changeAccess.ExpiryDate
        });

        var autoResetEvent = new AutoResetEvent(false);
        var response = await WaitForReply(autoResetEvent, "user_change_access_replies", id);

        Logger.LogDebug($"Got reply with id {id} from IdentityServer when updating user information");

        return response;
    }

    private async Task HideAuthorEbooks(int authorId)
    {
        await Context.EBooks
            .Where(e => e.AuthorId == authorId)
            .ExecuteUpdateAsync(setters =>
                setters.SetProperty(e => e.IsHidden, true).SetProperty(e => e.HiddenDate, DateTime.UtcNow));
    }
    
    private async Task UnhideAuthorEbooks(int authorId)
    {
        await Context.EBooks
            .Where(e => e.AuthorId == authorId)
            .ExecuteUpdateAsync(setters =>
                setters.SetProperty(e => e.IsHidden, false).SetProperty(e => e.HiddenDate, (DateTime?)null));
    }

    private async Task ValidateUpdateProfile(Db.Entities.User user, UserUpdateRequest request)
    {
        if (request.Email != user.Email)
        {
            if (await Context.Users.AnyAsync(e => e.Id != user.Id && e.Email == request.Email))
            {
                throw new AppException("Email is taken!");
            }
        }

        if (request.Username != user.Username)
        {
            if (await Context.Users.AnyAsync(e => e.Id != user.Id && e.Username == request.Username))
            {
                throw new AppException("Username is taken");
            }
        }
    }
}