#nullable enable
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects.Image;
using Wordsmith.Models.RequestObjects.Statistics;
using Wordsmith.Models.RequestObjects.User;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services.User;

public interface IUserService : IWriteService<UserDto, Db.Entities.User, UserSearchObject, UserInsertRequest, UserUpdateRequest>
{
    public Task<EntityResult<UserLoginDto>> Login(UserLoginRequest login);
    
    public Task<EntityResult<UserDto>> UpdateProfile(UserUpdateRequest update, int userId);
    
    public Task<QueryResult<ImageDto>> GetProfileImage(int userId);
    
    public Task<EntityResult<UserDto>> UpdateProfileImage(ImageInsertRequest update, int userId);
    
    public Task<QueryResult<UserLoginDto>> Refresh(string? bearerToken, int userId);
    
    public Task<QueryResult<UserLoginDto>> VerifyLogin(string? bearerToken,
        int userId);
    
    public Task<EntityResult<UserDto>> ChangeAccess(int userId, UserChangeAccessRequest changeAccess,
        int adminId);
    
    public Task<QueryResult<UserStatisticsDto>> GetUserStatistics(int userId);
    
    public Task<QueryResult<UserRegistrationStatisticsDto>> GetRegistrationStatistics(StatisticsRequest request);
    
    public Task<QueryResult<UserPurchasesStatisticsDto>> GetPurchaseStatistics(StatisticsRequest request);
}