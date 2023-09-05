using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.DataAccess.Services;
using Wordsmith.Models;
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
    [HttpPut("profile")]
    public Task<ActionResult<UserDto>> UpdateProfile(UserUpdateRequest update)
    {
        var userId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "user_ref_id");
        
        if (WriteService is IUserService userService)
        {
            return userService.UpdateProfile(userId?.Value, update);
        }
        
        throw new Exception("User service is unavailable");
    }

    [HttpGet("login")]
    public Task<ActionResult<UserLoginDto>> Login([FromBody] UserLoginRequest login)
    {
        if (WriteService is IUserService userService)
        {
            return userService.Login(login);
        }

        throw new Exception("User service is unavailable");
    }

    [HttpGet("refresh")]
    public Task<ActionResult<UserLoginDto>> Refresh([FromQuery] string client)
    {
        var bearerToken = HttpContext.Request.Headers["Authorization"];
        
        if (WriteService is IUserService userService)
        {
            return userService.Refresh(bearerToken, client);
        }
        
        throw new Exception("User service is unavailable");
    }
}

