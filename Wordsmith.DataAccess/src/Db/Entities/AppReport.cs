using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

#pragma warning disable CA1825

namespace Wordsmith.DataAccess.Db.Entities;

[Table("app_reports")]
public class AppReport : IEntity
{
    [Key] public int Id { get; set; }

    [StringLength(400)]
    public string Content { get; set; }
    
    public DateTime SubmissionDate { get; set; }

    public bool IsClosed { get; set; }

    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))] public virtual User User { get; set; }
}