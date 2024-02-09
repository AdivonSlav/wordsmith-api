using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services.Genre;

public interface IGenreService : IReadService<GenreDto, GenreSearchObject>
{
    
}