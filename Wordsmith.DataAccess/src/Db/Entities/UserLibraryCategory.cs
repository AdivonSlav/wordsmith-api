using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Wordsmith.DataAccess.Db.Entities;

[Table("user_library_categories")]
[Index(nameof(Name))]
public class UserLibraryCategory : IEntity
{
    [Key]
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public int UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; }
}