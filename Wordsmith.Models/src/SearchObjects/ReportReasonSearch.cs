using Swashbuckle.AspNetCore.Annotations;
using Wordsmith.Models.Enums;

namespace Wordsmith.Models.SearchObjects;

public class ReportReasonSearch : SearchObject
{
    [SwaggerParameter("Type of the report")]
    public ReportType? Type { get; set; }
}