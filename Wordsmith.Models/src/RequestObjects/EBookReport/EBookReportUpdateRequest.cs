using System.ComponentModel.DataAnnotations;

namespace Wordsmith.Models.RequestObjects.EBookReport;

public class EBookReportUpdateRequest
{
    [Required]
    public bool IsClosed { get; set; } 
}