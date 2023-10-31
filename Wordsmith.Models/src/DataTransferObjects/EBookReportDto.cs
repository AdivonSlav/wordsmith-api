namespace Wordsmith.Models.DataTransferObjects;

public class EBookReportDto
{
    public int Id { get; set; }

    public EBookDto ReportedEBook { get; set; }

    public ReportDetailsDto ReportDetails { get; set; }
}