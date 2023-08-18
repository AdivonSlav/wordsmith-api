namespace Wordsmith.Models;

public class UserReportDto
{
    public int Id { get; set; }

    public int UserId { get; set; } // The user who is being reported

    public ReportDetailsDto ReportDetailsDto { get; set; }
}