using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace Wordsmith.DataAccess.Db.Entities;

[Table("favorite_ebooks")]
public class FavoriteEBook : IEntity
{
    [Key] public int Id { get; set; }
    
    public int EBookId { get; set; }
    
    public int UserId { get; set; }
    
    [ForeignKey(nameof(EBookId))] public virtual EBook EBook { get; set; }
    
    [ForeignKey(nameof(UserId))] public virtual User User { get; set; }
}