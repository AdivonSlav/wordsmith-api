using Microsoft.AspNetCore.Mvc;
using Wordsmith.DataAccess.Services.UserLibraryCategory;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.API.Controllers;

[ApiController]
[Route("user-library-categories")]
public class UserLibraryCategoryController : ReadController<UserLibraryCategoryDto, UserLibraryCategorySearchObject>
{
    public UserLibraryCategoryController(IUserLibraryCategoryService userLibraryCategoryService) : base(userLibraryCategoryService) { }
}