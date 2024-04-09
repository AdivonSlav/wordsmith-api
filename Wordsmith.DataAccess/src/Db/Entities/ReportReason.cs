using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Wordsmith.Models.Enums;

#pragma warning disable CA1825

namespace Wordsmith.DataAccess.Db.Entities;

[Table("report_reasons")]
public class ReportReason : IEntity
{
    [Key] public int Id { get; set; }

    [StringLength(100)]
    public string Reason { get; set; }
    
    public ReportType Type { get; set; }
}