using Microsoft.AspNetCore.Mvc;
using Wordsmith.DataAccess.Services.UserLibraryCategory;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.API.Controllers;

[ApiController]
[Route("user-library-categories")]
public class UserLibraryCategoriesController : ReadController<UserLibraryCategoryDto, UserLibraryCategorySearchObject>
{
    public UserLibraryCategoriesController(IUserLibraryCategoryService userLibraryCategoryService) : base(userLibraryCategoryService) { }
}