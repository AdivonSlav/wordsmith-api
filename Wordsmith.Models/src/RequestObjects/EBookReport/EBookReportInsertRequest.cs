using System.ComponentModel.DataAnnotations;

namespace Wordsmith.Models.RequestObjects.EBookReport;

public class EBookReportInsertRequest
{
    [Required]
    public int ReportedEBookId { get; set; }
    
    public int? ReporterUserId { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Content { get; set; }
    
    [Required]
    public int ReportReasonId { get; set; }
}