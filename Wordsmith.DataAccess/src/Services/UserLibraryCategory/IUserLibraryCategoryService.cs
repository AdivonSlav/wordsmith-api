using Microsoft.AspNetCore.Mvc;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects.UserLibraryCategory;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services.UserLibraryCategory;

public interface IUserLibraryCategoryService : IWriteService<UserLibraryCategoryDto, Db.Entities.UserLibraryCategory,UserLibraryCategorySearchObject, UserLibraryCategoryInsertRequest, UserLibraryCategoryUpdateRequest>
{
    public Task<ActionResult<UserLibraryCategoryDto>> AddToCategory(UserLibraryCategoryInsertRequest insertRequest);
}