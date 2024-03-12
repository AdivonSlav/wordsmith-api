using System.ComponentModel;
using Swashbuckle.AspNetCore.Annotations;

namespace Wordsmith.Models.SearchObjects;

public class SearchObject
{
    [SwaggerParameter("The requested page")]
    [DefaultValue(1)]
    public int Page { get; } = 1;

    [SwaggerParameter("The item count that a page should have")]
    [DefaultValue(100)]
    public int PageSize { get; } = 100;
    
    [SwaggerParameter("Whether the result should be ordered by a specific property, e.g. PropertyA:asc")]
    public string? OrderBy { get; set; }
    
    [SwaggerParameter("A list of properties to include with the query")]
    public IEnumerable<string>? Includes { get; set; }
}