using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.DataAccess.Services;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.API.Controllers;

[ApiController]
[Route("user-library")]
public class UserLibraryController : WriteController<UserLibraryDto, UserLibrary, UserLibrarySearchObject, UserLibraryInsertRequest,UserLibraryUpdateRequest>
{
    public UserLibraryController(IUserLibraryService userLibraryService) : base(userLibraryService) { }

    [Authorize("All")]
    public override Task<ActionResult<UserLibraryDto>> Insert(UserLibraryInsertRequest insert)
    {
        var userRefId = HttpContext.User.Claims.First(c => c.Type == "user_ref_id");
        insert.UserId = int.Parse(userRefId.Value);
        
        return base.Insert(insert);
    }
}