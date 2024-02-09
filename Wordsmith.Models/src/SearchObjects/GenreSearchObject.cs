using Swashbuckle.AspNetCore.Annotations;

namespace Wordsmith.Models.SearchObjects;

public class GenreSearchObject : SearchObject
{
    [SwaggerParameter("Name of the genre")]
    public string? Name { get; set; }
}