using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace Wordsmith.DataAccess.Db.Entities;

[Table("ebook_chapters")]
public class EBookChapter : IEntity
{
    [Key] public int Id { get; set; }
    
    [StringLength(400)]
    public string ChapterName { get; set; }
    
    public int ChapterNumber { get; set; }
    
    public int EBookId { get; set; }
    
    [ForeignKey(nameof(EBookId))] public virtual EBook EBook { get; set; }
}