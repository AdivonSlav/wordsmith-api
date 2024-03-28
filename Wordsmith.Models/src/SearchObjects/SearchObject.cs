using System.ComponentModel;
using Swashbuckle.AspNetCore.Annotations;

namespace Wordsmith.Models.SearchObjects;

public class SearchObject
{
    [SwaggerParameter("The requested page")]
    [DefaultValue(1)]
    public int Page { get; init; } = 1;

    [SwaggerParameter("The item count that a page should have")]
    [DefaultValue(100)]
    public int PageSize { get; init; } = 100;
    
    [SwaggerParameter("Whether the result should be ordered by a specific property, e.g. PropertyA:asc")]
    public string? OrderBy { get; set; }
}