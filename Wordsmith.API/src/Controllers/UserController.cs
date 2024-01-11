using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.DataAccess.Services;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.API.Controllers;

[ApiController]
[Route("users")]
public class UserController : WriteController<UserDto, User, SearchObject, UserInsertRequest, UserUpdateRequest>
{
    public UserController(IUserService userService)
        : base(userService) { }

    [HttpPost("register")]
    public override Task<ActionResult<UserDto>> Insert([FromBody] UserInsertRequest insert)
    {
        return base.Insert(insert);
    }

    [NonAction]
    public override Task<ActionResult<UserDto>> Update(int id, UserUpdateRequest update)
    {
        return base.Update(id, update);
    }

    [Authorize("All")]
    [HttpGet("profile")]
    public async Task<ActionResult<QueryResult<UserDto>>> GetProfile()
    {
        var user = await ((WriteService as IUserService)!).GetUserFromClaims(HttpContext.User.Claims);

        return await base.GetById(user.Id);
    }
    
    [Authorize("All")]
    [HttpPut("profile")]
    public Task<ActionResult<UserDto>> UpdateProfile(UserUpdateRequest update)
    {
        return ((WriteService as IUserService)!).UpdateProfile(update, HttpContext.User.Claims);
    }

    [Authorize("All")]
    [HttpGet("profile/image")]
    public Task<ActionResult<QueryResult<ImageDto>>> GetProfileImage()
    {
        return ((WriteService as IUserService)!).GetProfileImage(HttpContext.User.Claims);
    }
    
    [Authorize("All")]
    [HttpPut("profile/image")]
    public Task<ActionResult<ImageDto>> UpdateProfileImage([FromBody] ImageInsertRequest update)
    {
        return ((WriteService as IUserService)!).UpdateProfileImage(update, HttpContext.User.Claims);
    }

    [HttpPost("login")]
    public Task<ActionResult<UserLoginDto>> Login([FromBody] UserLoginRequest login)
    {
        return ((WriteService as IUserService)!).Login(login);
    }

    [HttpGet("refresh")]
    public Task<ActionResult<UserLoginDto>> Refresh([FromQuery] string client)
    {
        var bearerToken = HttpContext.Request.Headers["Authorization"];

        return ((WriteService as IUserService)!).Refresh(bearerToken, client);
    }

    [Authorize("AdminOperations")]
    [HttpPut("{id:int}/change-access")]
    public Task<ActionResult> ChangeAccess(int id, [FromBody] UserChangeAccessRequest changeAccess)
    {
        return ((WriteService as IUserService)!).ChangeAccess(id, changeAccess, HttpContext.User.Claims);
    }
}