using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace Wordsmith.DataAccess.Db.Entities;

[Table("report_details")]
public class ReportDetails : IEntity
{
    [Key] public int Id { get; set; }
    
    [StringLength(200)]
    public string Content { get; set; }

    public DateTime SubmissionDate { get; set; }

    public bool IsClosed { get; set; }
    
    public int ReportReasonId { get; set; }
    
    public int UserId { get; set; }

    [ForeignKey(nameof(ReportReasonId))] public virtual ReportReason ReportReason { get; set; }
    
    [ForeignKey(nameof(UserId))] public virtual User Reporter { get; set; }
}