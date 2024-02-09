using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services;

public interface IGenreService : IReadService<GenreDto, GenreSearchObject>
{
    
}