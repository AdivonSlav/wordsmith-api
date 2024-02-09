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
    
    [SwaggerParameter("The date when the report was made")]
    public DateTime? ReportDate { get; set; }
}