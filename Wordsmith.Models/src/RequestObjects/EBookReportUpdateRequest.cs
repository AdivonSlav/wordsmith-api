using System.ComponentModel.DataAnnotations;

namespace Wordsmith.Models.RequestObjects;

public class EBookReportUpdateRequest
{
    [Required]
    public bool IsClosed { get; set; } 
}