using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace Wordsmith.DataAccess.Db.Entities;

[Table("author_follows")]
public class AuthorFollow : IEntity
{
    [Key] public int Id { get; set; }
    
    public int AuthorUserId { get; set; }
    
    public int UserId { get; set; }
    
    [ForeignKey(nameof(AuthorUserId))] public virtual User AuthorUser { get; set; }
    
    [ForeignKey(nameof(UserId))] public virtual User User { get; set; }
}