using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace Wordsmith.DataAccess.Db.Entities;

[Table("comments")]
public class Comment : IEntity
{
    [Key] public int Id { get; set; }
    
    [StringLength(400)]
    public string Content { get; set; }
    
    public DateTime DateAdded { get; set; }
    
    public bool IsShown { get; set; }
    
    public int? EBookChapterId { get; set; }
    
    public int EBookId { get; set; }
    
    public int UserId { get; set; }
    
    [ForeignKey(nameof(UserId))] public virtual User User { get; set; }
    
    [ForeignKey(nameof(EBookId))] public virtual EBook EBook { get; set; }
}