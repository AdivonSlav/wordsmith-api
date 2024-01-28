using Swashbuckle.AspNetCore.Annotations;

namespace Wordsmith.Models.SearchObjects;

public class SearchObject
{
    [SwaggerParameter("The requested page")]
    public int? Page { get; set; }
    
    [SwaggerParameter("The item count that a page should have")]
    public int? PageSize { get; set; }
    
    [SwaggerParameter("Whether the result should be ordered by a specific property, e.g. PropertyA:asc")]
    public string? OrderBy { get; set; }
}