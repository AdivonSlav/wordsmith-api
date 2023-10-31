namespace Wordsmith.Models.SearchObjects;

public class EBookReportSearchObject : SearchObject
{
    public int? ReportedEBookId { get; set; }
    
    public bool? IsClosed { get; set; }
}