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
    public override Task<ActionResult<UserDto>> Insert(UserInsertRequest insert)
    {
        return base.Insert(insert);
    }
}
