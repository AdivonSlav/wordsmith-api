using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.DataAccess.Services.UserLibraryCategory;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects.UserLibraryCategory;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.API.Controllers;

[ApiController]
[Route("user-library-categories")]
public class UserLibraryCategoriesController : WriteController<UserLibraryCategoryDto, UserLibraryCategory, UserLibraryCategorySearchObject, UserLibraryCategoryInsertRequest, UserLibraryCategoryUpdateRequest>
{
    public UserLibraryCategoriesController(IUserLibraryCategoryService userLibraryCategoryService) : base(userLibraryCategoryService) { }

    [SwaggerOperation("Add a user's library entry to an existing category or create it if it doesn't exist")]
    [Authorize("All")]
    [HttpPost]
    public async Task<ActionResult<EntityResult<UserLibraryCategoryDto>>> AddToCategory(UserLibraryCategoryInsertRequest insert)
    {
        insert.UserId = GetAuthUserId();
        var result = await (WriteService as IUserLibraryCategoryService)!.AddToCategory(insert);

        return Ok(result);
    }

    [NonAction]
    public override Task<ActionResult<EntityResult<UserLibraryCategoryDto>>> Insert(UserLibraryCategoryInsertRequest insert)
    {
        return base.Insert(insert);
    }

    [NonAction]
    public override Task<ActionResult<EntityResult<UserLibraryCategoryDto>>> Update(int id, UserLibraryCategoryUpdateRequest update)
    {
        return base.Update(id, update);
    }
}