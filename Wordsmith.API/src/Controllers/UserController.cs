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
    public Task<ActionResult<QueryResult<UserDto>>> GetProfile()
    {
        var userId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "user_ref_id");
        
        return base.GetById(int.Parse(userId!.Value));
    }
    
    [Authorize("All")]
    [HttpPut("profile")]
    public Task<ActionResult<UserDto>> UpdateProfile(UserUpdateRequest update)
    {
        var userId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "user_ref_id");

        return ((WriteService as IUserService)!).UpdateProfile(userId!.Value, update);
    }

    [Authorize("All")]
    [HttpGet("profile/image")]
    public Task<ActionResult<QueryResult<ImageDto>>> GetProfileImage()
    {
        var userId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "user_ref_id");

        return ((WriteService as IUserService)!).GetProfileImage(userId!.Value);
    }
    
    [Authorize("All")]
    [HttpPut("profile/image")]
    public Task<ActionResult<ImageDto>> UpdateProfileImage([FromBody] ImageInsertRequest update)
    {
        var userId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "user_ref_id");

        return ((WriteService as IUserService)!).UpdateProfileImage(userId!.Value, update);
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
        var adminId = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "user_ref_id");

        return ((WriteService as IUserService)!).ChangeAccess(adminId!.Value, id, changeAccess);
    }
}