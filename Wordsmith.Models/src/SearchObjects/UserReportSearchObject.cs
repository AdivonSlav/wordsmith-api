using Swashbuckle.AspNetCore.Annotations;

namespace Wordsmith.Models.SearchObjects;

public class UserReportSearchObject : SearchObject
{
    [SwaggerParameter("The user which was reported")]
    public int? ReportedUserId { get; set; }
    
    [SwaggerParameter("Whether the report has been closed")]
    public bool? IsClosed { get; set; }
    
    [SwaggerParameter("The reason for the report")]
    public string? Reason { get; set; }
    
    [SwaggerParameter("Start date for reports")]
    public DateTime? StartDate { get; set; }
    
    [SwaggerParameter("End date for reports")]
    public DateTime? EndDate { get; set; }
}