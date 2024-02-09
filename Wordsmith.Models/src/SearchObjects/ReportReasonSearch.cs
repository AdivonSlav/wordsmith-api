using Swashbuckle.AspNetCore.Annotations;

namespace Wordsmith.Models.SearchObjects;

public class ReportReasonSearch : SearchObject
{
    [SwaggerParameter("Subject of the report")]
    public string? Subject { get; set; }
}