namespace Wordsmith.Models;

public class ReportDetailsDto
{
    public int Id { get; set; }

    public UserDto Reporter { get; set; } // The user who made the report

    public ReportReasonDto ReportReason { get; set; }

    public string Content { get; set; }

    public DateTime SubmissionDate { get; set; }

    public bool IsClosed { get; set; }
}