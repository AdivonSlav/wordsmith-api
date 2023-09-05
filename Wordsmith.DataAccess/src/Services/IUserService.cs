#nullable enable
using Microsoft.AspNetCore.Mvc;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Models;
using Wordsmith.Models.RequestObjects;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services;

public interface IUserService : IWriteService<UserDto, User, SearchObject, UserInsertRequest, UserUpdateRequest>
{
    public Task<ActionResult<UserLoginDto>> Login(UserLoginRequest login);
    public Task<ActionResult<UserDto>> UpdateProfile(string userIdStr, UserUpdateRequest update);
    public Task<ActionResult<UserLoginDto>> Refresh(string? bearerToken, string client);
    public Task<ActionResult> ChangeAccess(string adminIdStr, int userId,  UserChangeAccessRequest changeAccess);
}