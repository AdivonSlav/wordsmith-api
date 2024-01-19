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
    public override async Task<ActionResult<UserDto>> Insert([FromBody] UserInsertRequest insert)
    {
        return await base.Insert(insert);
    }

    [NonAction]
    public override async Task<ActionResult<UserDto>> Update(int id, UserUpdateRequest update)
    {
        return await base.Update(id, update);
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
    public async Task<ActionResult<UserDto>> UpdateProfile(UserUpdateRequest update)
    {
        return await ((WriteService as IUserService)!).UpdateProfile(update, HttpContext.User.Claims);
    }

    [Authorize("All")]
    [HttpGet("profile/image")]
    public async Task<ActionResult<QueryResult<ImageDto>>> GetProfileImage()
    {
        return await ((WriteService as IUserService)!).GetProfileImage(HttpContext.User.Claims);
    }
    
    [Authorize("All")]
    [HttpPut("profile/image")]
    public async Task<ActionResult<ImageDto>> UpdateProfileImage([FromBody] ImageInsertRequest update)
    {
        return await ((WriteService as IUserService)!).UpdateProfileImage(update, HttpContext.User.Claims);
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserLoginDto>> Login([FromBody] UserLoginRequest login)
    {
        return await ((WriteService as IUserService)!).Login(login);
    }

    [HttpGet("login/refresh")]
    public async Task<ActionResult<QueryResult<UserLoginDto>>> Refresh([FromQuery] int id)
    {
        var bearerToken = HttpContext.Request.Headers["Authorization"];

        return await ((WriteService as IUserService)!).Refresh(bearerToken, id);
    }

    [Authorize("AdminOperations")]
    [HttpPut("{id:int}/change-access")]
    public async Task<ActionResult> ChangeAccess(int id, [FromBody] UserChangeAccessRequest changeAccess)
    {
        return await ((WriteService as IUserService)!).ChangeAccess(id, changeAccess, HttpContext.User.Claims);
    }
}