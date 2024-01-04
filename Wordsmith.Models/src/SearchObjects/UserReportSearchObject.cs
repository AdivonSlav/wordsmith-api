namespace Wordsmith.Models.SearchObjects;

public class UserReportSearchObject : SearchObject
{
    public int? ReportedUserId { get; set; }
    
    public bool? IsClosed { get; set; }
    
    public string? Reason { get; set; }
    
    public DateTime? ReportDate { get; set; }
}