using Swashbuckle.AspNetCore.Annotations;

namespace Wordsmith.Models.SearchObjects;

public class AppReportSearchObject : SearchObject
{
    [SwaggerParameter("The user who made the report")]
    public int? UserId { get; set; }
    
    [SwaggerParameter("Whether the report has been closed")]
    public bool? IsClosed { get; set; }
    
    [SwaggerParameter("Start date for reports")]
    public DateTime? StartDate { get; set; }
    
    [SwaggerParameter("End date for reports")]
    public DateTime? EndDate { get; set; }
}