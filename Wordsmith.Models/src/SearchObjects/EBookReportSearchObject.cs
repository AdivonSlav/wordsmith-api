using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Wordsmith.Models.SearchObjects;

public class EBookReportSearchObject : SearchObject
{
    [SwaggerParameter("ID of the ebook being reported")]
    public int? ReportedEBookId { get; set; }
    
    [SwaggerParameter("Whether the report has been closed")]
    public bool? IsClosed { get; set; }
    
    [SwaggerParameter("Reason for reporting")]
    [StringLength(30)]
    public string? Reason { get; set; }
    
    [SwaggerParameter("Start date for reports")]
    public DateTime? StartDate { get; set; }
    
    [SwaggerParameter("End date for reports")]
    public DateTime? EndDate { get; set; }
}