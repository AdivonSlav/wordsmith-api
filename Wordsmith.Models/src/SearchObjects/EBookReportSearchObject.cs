using System.ComponentModel.DataAnnotations;

namespace Wordsmith.Models.SearchObjects;

public class EBookReportSearchObject : SearchObject
{
    public int? ReportedEBookId { get; set; }
    
    public bool? IsClosed { get; set; }
    
    [StringLength(30)]
    public string? Reason { get; set; }
    
    public DateTime? ReportDate { get; set; }
}