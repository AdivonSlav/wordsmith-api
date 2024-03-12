using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace Wordsmith.DataAccess.Db.Entities;

[Table("user_bans")]
public class UserBan : IEntity
{
    [Key] public int Id { get; set; }
    
    public DateTime BannedDate { get; set; }
    
    public DateTime? ExpiryDate { get; set; }
    
    public int AdminId { get; set; }
    
    public int UserId { get; set; }
    
    [ForeignKey(nameof(AdminId))] public virtual User Admin { get; set; }
    
    [ForeignKey(nameof(UserId))] public virtual User User { get; set; }
}