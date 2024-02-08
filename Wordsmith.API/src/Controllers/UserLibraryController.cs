using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.DataAccess.Services;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects;
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

    [Authorize("All")]
    public override Task<ActionResult<QueryResult<UserLibraryDto>>> Get(UserLibrarySearchObject search)
    {
        var userRefId = HttpContext.User.Claims.First(c => c.Type == "user_ref_id");
        search.UserId = int.Parse(userRefId.Value);
        
        return base.Get(search);
    }

    // [Authorize("All")]
    // [HttpGet("{eBookId:int}")]
    // public async Task<ActionResult<QueryResult<UserLibraryDto>>> GetLibraryEntry(int eBookId)
    // {
    //     var userRefId = HttpContext.User.Claims.First(c => c.Type == "user_ref_id");
    //
    //     return await (WriteService as IUserLibraryService)!.GetLibraryEntry(int.Parse(userRefId.Value), eBookId);
    // }

    [Authorize("All")]
    public override async Task<ActionResult<UserLibraryDto>> Insert(UserLibraryInsertRequest insert)
    {
        var userRefId = HttpContext.User.Claims.First(c => c.Type == "user_ref_id");
        insert.UserId = int.Parse(userRefId.Value);
        
        return await base.Insert(insert);
    }
    
    [Authorize("All")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<string>> Delete(int id)
    {
        var userRefId = HttpContext.User.Claims.First(c => c.Type == "user_ref_id");
        
        return await WriteService.Delete(id, int.Parse(userRefId.Value));
    }
}