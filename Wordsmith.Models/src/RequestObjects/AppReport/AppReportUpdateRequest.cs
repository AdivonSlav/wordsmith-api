using System.ComponentModel.DataAnnotations;

namespace Wordsmith.Models.RequestObjects.AppReport;

public class AppReportUpdateRequest
{
    [Required]
    public bool IsClosed { get; set; }    
}