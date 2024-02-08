using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wordsmith.DataAccess.Db.Entities;

[Table("favorite_ebooks")]
public class FavoriteEBook
{
    public int EBookId { get; set; }
    
    public int UserId { get; set; }
    
    [ForeignKey(nameof(EBookId))] public virtual EBook EBook { get; set; }
    
    [ForeignKey(nameof(UserId))] public virtual User User { get; set; }
}