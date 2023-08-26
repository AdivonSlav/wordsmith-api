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

