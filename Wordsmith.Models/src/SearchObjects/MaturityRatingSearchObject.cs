using Swashbuckle.AspNetCore.Annotations;

namespace Wordsmith.Models.SearchObjects;

public class MaturityRatingSearchObject : SearchObject
{
    [SwaggerParameter("Name of the maturity rating")]
    public string? Name { get; set; }
}