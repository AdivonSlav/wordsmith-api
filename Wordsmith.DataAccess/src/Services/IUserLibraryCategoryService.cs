using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services;

public interface IUserLibraryCategoryService : IReadService<UserLibraryCategoryDto, UserLibraryCategorySearchObject>
{
    
}