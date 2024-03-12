using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace Wordsmith.DataAccess.Db.Entities;

[Table("user_library_categories")]
[Index(nameof(Name))]
public class UserLibraryCategory : IEntity
{
    [Key]
    public int Id { get; set; }
    
    [StringLength(255)]
    public string Name { get; set; }
    
    public int UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; }
}