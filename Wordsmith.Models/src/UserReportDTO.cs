namespace Wordsmith.Models;

public class UserReportDTO
{
    public int Id { get; set; }
        
    public int UserId { get; set; } // The user who is being reported

    public ReportDetailsDTO ReportDetailsDto { get; set; }
}