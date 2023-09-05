using System.ComponentModel.DataAnnotations;

namespace Wordsmith.Models.RequestObjects;

public class UserReportUpdateRequest
{
    [Required]
    public bool IsClosed { get; set; }    
}
