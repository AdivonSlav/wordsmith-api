using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wordsmith.DataAccess.Db.Entities;

[Table("user_reports")]
public class UserReport
{
    [Key] public int Id { get; set; }

    public int ReportedUserId { get; set; } // The user who is being reported

    public int ReportDetailsId { get; set; }

    [ForeignKey((nameof(ReportedUserId)))] public virtual User ReportedUser { get; set; }
    
    [ForeignKey("ReportDetailsId")] public virtual ReportDetails ReportDetails { get; set; }
}