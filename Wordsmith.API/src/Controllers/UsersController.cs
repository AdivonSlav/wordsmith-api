using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.DataAccess.Services.User;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects.Image;
using Wordsmith.Models.RequestObjects.User;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.API.Controllers;

[ApiController]
[Route("users")]
public class UsersController : WriteController<UserDto, User, SearchObject, UserInsertRequest, UserUpdateRequest>
{
    public UsersController(IUserService userService)
        : base(userService) { }

    [SwaggerOperation("Adds a new user")]
    [HttpPost("register")]
    public override async Task<ActionResult<UserDto>> Insert([FromBody] UserInsertRequest insert)
    {
        return await base.Insert(insert);
    }

    [NonAction]
    public override async Task<ActionResult<UserDto>> Update(int id, UserUpdateRequest update)
    {
        return await base.Update(id, update);
    }

    [SwaggerOperation("Get the profile of the user from the provided bearer token")]
    [Authorize("All")]
    [HttpGet("profile")]
    public async Task<ActionResult<QueryResult<UserDto>>> GetProfile()
    {
        var userId = GetAuthUserId();
        return await base.GetById(userId);
    }
    
    [SwaggerOperation("Updates the profile of the user from the provided bearer token")]
    [Authorize("All")]
    [HttpPut("profile")]
    public async Task<ActionResult<UserDto>> UpdateProfile(UserUpdateRequest update)
    {
        var userId = GetAuthUserId();
        return await ((WriteService as IUserService)!).UpdateProfile(update, userId);
    }

    [SwaggerOperation("Gets the profile image of the user from the provided bearer token")]
    [Authorize("All")]
    [HttpGet("profile/image")]
    public async Task<ActionResult<QueryResult<ImageDto>>> GetProfileImage()
    {
        var userId = GetAuthUserId();
        return await ((WriteService as IUserService)!).GetProfileImage(userId);
    }
    
    [SwaggerOperation("Updates the profile image of the user from the provided bearer token")]
    [Authorize("All")]
    [HttpPut("profile/image")]
    public async Task<ActionResult<ImageDto>> UpdateProfileImage([FromBody] ImageInsertRequest update)
    {
        var userId = GetAuthUserId();
        return await ((WriteService as IUserService)!).UpdateProfileImage(update, userId);
    }

    [SwaggerOperation("Performs a user login with the provided credentials")]
    [HttpPost("login")]
    public async Task<ActionResult<UserLoginDto>> Login([FromBody] UserLoginRequest login)
    {
        return await ((WriteService as IUserService)!).Login(login);
    }

    [SwaggerOperation("Refreshes the provided refresh token based on the user id")]
    [HttpGet("login/refresh")]
    public async Task<ActionResult<QueryResult<UserLoginDto>>> Refresh([FromQuery] int id)
    {
        var bearerToken = HttpContext.Request.Headers["Authorization"];

        return await ((WriteService as IUserService)!).Refresh(bearerToken, id);
    }

    [SwaggerOperation("Verifies whether the provided access token is still valid")]
    [HttpGet("login/verify")]
    public async Task<ActionResult<QueryResult<UserLoginDto>>> VerifyLogin()
    {
        var userId = GetAuthUserId();
        var bearerToken = HttpContext.Request.Headers["Authorization"];
        
        return await ((WriteService as IUserService)!).VerifyLogin(bearerToken, userId);
    }

    [SwaggerOperation("Toggles access for a user based on the provided userId and bearer token")]
    [Authorize("AdminOperations")]
    [HttpPut("{userId:int}/change-access")]
    public async Task<ActionResult> ChangeAccess(int userId, [FromBody] UserChangeAccessRequest changeAccess)
    {
        var adminId = GetAuthUserId();
        return await ((WriteService as IUserService)!).ChangeAccess(userId, changeAccess, adminId);
    }
}