using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace Wordsmith.DataAccess.Db.Entities;

[Table("ebook_reports")]
public class EBookReport : IEntity
{
    [Key] public int Id { get; set; }

    public int EBookId { get; set; }
    
    public int ReportDetailsId { get; set; }

    [ForeignKey(nameof(EBookId))] public virtual EBook ReportedEBook { get; set; }
    
    [ForeignKey(nameof(ReportDetailsId))] public virtual ReportDetails ReportDetails { get; set; }
}