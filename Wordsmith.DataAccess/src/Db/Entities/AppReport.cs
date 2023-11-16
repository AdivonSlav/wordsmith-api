using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#pragma warning disable CA1825

namespace Wordsmith.DataAccess.Db.Entities;

[Table("app_reports")]
public class AppReport
{
    [Key] public int Id { get; set; }

    public string Content { get; set; }

    public bool IsClosed { get; set; }

    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))] public virtual User User { get; set; }
}