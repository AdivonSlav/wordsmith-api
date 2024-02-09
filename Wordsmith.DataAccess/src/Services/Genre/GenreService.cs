using AutoMapper;
using Wordsmith.DataAccess.Db;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services.Genre;

public class GenreService : ReadService<GenreDto, Db.Entities.Genre, GenreSearchObject>, IGenreService
{
    public GenreService(DatabaseContext context, IMapper mapper) : base(context, mapper) { }
}