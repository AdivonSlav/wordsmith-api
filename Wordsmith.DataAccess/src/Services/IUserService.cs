#nullable enable
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services;

public interface IUserService : IWriteService<UserDto, User, SearchObject, UserInsertRequest, UserUpdateRequest>
{
    public Task<ActionResult<UserLoginDto>> Login(UserLoginRequest login);
    public Task<ActionResult<UserDto>> UpdateProfile(UserUpdateRequest update, IEnumerable<Claim> userClaims);
    public Task<ActionResult<QueryResult<ImageDto>>> GetProfileImage(IEnumerable<Claim> userClaims);
    public Task<ActionResult<ImageDto>> UpdateProfileImage(ImageInsertRequest update, IEnumerable<Claim> userClaims); 
    public Task<ActionResult<QueryResult<UserLoginDto>>> Refresh(string? bearerToken, int id);
    public Task<ActionResult<QueryResult<UserLoginDto>>> VerifyLogin(string? bearerToken,
        IEnumerable<Claim> userClaims);
    public Task<ActionResult> ChangeAccess(int userId, UserChangeAccessRequest changeAccess,
        IEnumerable<Claim> userClaims);
    public Task<User> GetUserFromClaims(IEnumerable<Claim> userClaims);
}