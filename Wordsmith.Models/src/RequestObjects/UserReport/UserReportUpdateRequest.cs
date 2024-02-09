using System.ComponentModel.DataAnnotations;

namespace Wordsmith.Models.RequestObjects.UserReport;

public class UserReportUpdateRequest
{
    [Required]
    public bool IsClosed { get; set; }    
}
