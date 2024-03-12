using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#pragma warning disable CA1825

namespace Wordsmith.DataAccess.Db.Entities;

[Table("report_reasons")]
[Index(nameof(Reason), IsUnique = true)]
public class ReportReason : IEntity
{
    [Key] public int Id { get; set; }

    [StringLength(100)]
    public string Reason { get; set; }
    
    [StringLength(100)]
    public string Subject { get; set; }
}