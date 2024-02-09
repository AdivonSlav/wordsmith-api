#nullable enable
using Microsoft.AspNetCore.Mvc;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects.Image;
using Wordsmith.Models.RequestObjects.User;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services.User;

public interface IUserService : IWriteService<UserDto, Db.Entities.User, SearchObject, UserInsertRequest, UserUpdateRequest>
{
    public Task<ActionResult<UserLoginDto>> Login(UserLoginRequest login);
    public Task<ActionResult<UserDto>> UpdateProfile(UserUpdateRequest update, int userId);
    public Task<ActionResult<QueryResult<ImageDto>>> GetProfileImage(int userId);
    public Task<ActionResult<ImageDto>> UpdateProfileImage(ImageInsertRequest update, int userId); 
    public Task<ActionResult<QueryResult<UserLoginDto>>> Refresh(string? bearerToken, int userId);
    public Task<ActionResult<QueryResult<UserLoginDto>>> VerifyLogin(string? bearerToken,
        int userId);
    public Task<ActionResult> ChangeAccess(int userId, UserChangeAccessRequest changeAccess,
        int adminId);
}