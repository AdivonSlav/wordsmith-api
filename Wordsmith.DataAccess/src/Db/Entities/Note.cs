using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wordsmith.DataAccess.Db.Entities;

[Table("notes")]
public class Note : IEntity
{
    [Key] public int Id { get; set; }
    
    public int Page { get; set; }
    
    public int CharBegin { get; set; }
    
    public int CharEnd { get; set; }
    
    public string Content { get; set; }
    
    public DateTime DateAdded { get; set; }
    
    public int EBookChapterId { get; set; }
    
    public int UserId { get; set; }
    
    [ForeignKey(nameof(EBookChapterId))] public virtual EBookChapter Chapter { get; set; }
    
    [ForeignKey(nameof(UserId))] public virtual User User { get; set; }
}