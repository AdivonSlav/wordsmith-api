namespace Wordsmith.Models;

public class UserReportDto
{
    public int Id { get; set; }

    public UserDto ReportedUser { get; set; }

    public ReportDetailsDto ReportDetails { get; set; }
}