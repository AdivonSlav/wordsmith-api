namespace Wordsmith.Models.SearchObjects;

public class UserReportSearchObject : SearchObject
{
    public int? ReportedUserId { get; set; }
    
    public bool? IsClosed { get; set; }
}