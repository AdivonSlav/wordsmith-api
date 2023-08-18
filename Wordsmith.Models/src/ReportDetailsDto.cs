namespace Wordsmith.Models;

public class ReportDetailsDto
{
    public int Id { get; set; }

    public int UserId { get; set; } // The user who made the report

    public ReportReasonDto ReportReasonDto { get; set; }

    public string Content { get; set; }

    public DateTime SubmissionDate { get; set; }

    public bool IsClosed { get; set; }
}