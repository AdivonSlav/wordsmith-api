using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services.UserLibraryCategory;

public interface IUserLibraryCategoryService : IReadService<UserLibraryCategoryDto, UserLibraryCategorySearchObject>
{
    
}