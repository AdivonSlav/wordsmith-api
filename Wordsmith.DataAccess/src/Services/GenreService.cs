using AutoMapper;
using Wordsmith.DataAccess.Db;
using Wordsmith.DataAccess.Db.Entities;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.DataAccess.Services;

public class GenreService : ReadService<GenreDto, Genre, GenreSearchObject>, IGenreService
{
    public GenreService(DatabaseContext context, IMapper mapper) : base(context, mapper) { }
}