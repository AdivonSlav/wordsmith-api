using Wordsmith.Models.Enums;

namespace Wordsmith.Models.DataTransferObjects;

public class ReportReasonDto
{
    public int Id { get; set; }

    public string Reason { get; set; }
    
    public string Type { get; set; }
}