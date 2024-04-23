using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Wordsmith.DataAccess.Db.Entities;

[Table("user_library_histories")]
public class UserLibraryHistory
{
    [Key]
    public int Id { get; set; }
    
    public int? EBookId { get; set; }
    
    public int? UserId { get; set; }
    
    public DateTime SyncDate { get; set; }
    
    [ForeignKey(nameof(EBookId))]
    [DeleteBehavior(DeleteBehavior.SetNull)]
    public virtual EBook EBook { get; set; }
    
    [ForeignKey(nameof(UserId))]
    [DeleteBehavior(DeleteBehavior.SetNull)]
    public virtual User User { get; set; }
}