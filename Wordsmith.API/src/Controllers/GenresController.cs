using Microsoft.AspNetCore.Mvc;
using Wordsmith.DataAccess.Services;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.SearchObjects;

namespace Wordsmith.API.Controllers;

[ApiController]
[Route("genres")]
public class GenresController : ReadController<GenreDto, GenreSearchObject>
{
    public GenresController(IGenreService genreService) : base(genreService) { }
}