using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.DataAccess.Services.UserLibrary;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects.UserLibrary;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.API.Controllers;

[ApiController]
[Route("user-libraries")]
public class UserLibrariesController : WriteController<UserLibraryDto, UserLibrary, UserLibrarySearchObject, UserLibraryInsertRequest,UserLibraryUpdateRequest>
{
    public UserLibrariesController(IUserLibraryService userLibraryService) : base(userLibraryService) { }

    [NonAction]
    public override Task<ActionResult<QueryResult<UserLibraryDto>>> GetById(int id)
    {
        return base.GetById(id);
    }

    [SwaggerOperation("Get a user's library entries based on a search criteria")]
    [Authorize("All")]
    public override Task<ActionResult<QueryResult<UserLibraryDto>>> Get(UserLibrarySearchObject search)
    {
        search.UserId = GetAuthUserId();
        
        return base.Get(search);
    }

    [SwaggerOperation("Add a new library entry for a user")]
    [Authorize("All")]
    public override async Task<ActionResult<EntityResult<UserLibraryDto>>> Insert(UserLibraryInsertRequest insert)
    {
        insert.UserId = GetAuthUserId();
        
        return await base.Insert(insert);
    }
    
    [SwaggerOperation("Remove a library entry for a user")]
    [Authorize("All")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<EntityResult<UserLibraryDto>>> Delete(int id)
    {
        return Ok(await WriteService.Delete(id));
    }
}