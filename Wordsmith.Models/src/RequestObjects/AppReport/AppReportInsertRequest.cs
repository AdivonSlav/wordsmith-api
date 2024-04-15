using System.ComponentModel.DataAnnotations;

namespace Wordsmith.Models.RequestObjects.AppReport;

public class AppReportInsertRequest
{
    [Required]
    [StringLength(200)]
    public string Content { get; set; }
    
    public int UserId { get; set; }
}