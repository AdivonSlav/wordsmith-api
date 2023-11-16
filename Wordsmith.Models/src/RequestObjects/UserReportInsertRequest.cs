using System.ComponentModel.DataAnnotations;

namespace Wordsmith.Models.RequestObjects;

public class UserReportInsertRequest
{
    [Required]
    public int ReportedUserId { get; set; }
    
    public int? ReporterUserId { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Content { get; set; }
    
    [Required]
    public int ReportReasonId { get; set; }
}