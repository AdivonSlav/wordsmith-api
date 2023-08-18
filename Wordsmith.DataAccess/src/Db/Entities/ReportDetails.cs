using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wordsmith.DataAccess.Db.Entities;

[Table("reportdetails")]
public class ReportDetails
{
    [Key] public int Id { get; set; }

    public int UserId { get; set; }

    public int ReportReasonId { get; set; }

    [ForeignKey("ReportReasonId")] public virtual ReportReason ReportReason { get; set; }

    public string Content { get; set; }

    public DateTime SubmissionDate { get; set; }

    public bool IsClosed { get; set; }
}