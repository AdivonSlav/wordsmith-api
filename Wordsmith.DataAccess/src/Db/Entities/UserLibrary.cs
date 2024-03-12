using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#pragma warning disable CA1825

namespace Wordsmith.DataAccess.Db.Entities;

[Table("user_libraries")]
[Index(nameof(IsRead))]
public class UserLibrary : IEntity
{
    [Key]
    public int Id { get; set; }
    
    public int EBookId { get; set; }
    
    public int UserId { get; set; }
    
    public DateTime SyncDate { get; set; }
    
    public bool IsRead { get; set; }
    
    public string ReadProgress { get; set; }
    
    public int? LastChapterId { get; set; }
    
    public int LastPage { get; set; }
    
    public int? UserLibraryCategoryId { get; set; }
    
    [ForeignKey(nameof(EBookId))] 
    public virtual EBook EBook { get; set; }
    
    [ForeignKey(nameof(UserId))] 
    public virtual User User { get; set; }
    
    [ForeignKey(nameof(LastChapterId))] 
    public virtual EBookChapter LastChapter { get; set; }
    
    [ForeignKey(nameof(UserLibraryCategoryId))] 
    [DeleteBehavior(DeleteBehavior.SetNull)]
    public virtual UserLibraryCategory UserLibraryCategory { get; set; }
}