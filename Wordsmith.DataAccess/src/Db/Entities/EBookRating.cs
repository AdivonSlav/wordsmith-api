using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wordsmith.DataAccess.Db.Entities;

[Table("ebook_ratings")]
public class EBookRating : IEntity
{
    [Key] public int Id { get; set; }
    
    public int Rating { get; set; }
    
    public DateTime RatingDate { get; set; }
    
    public int EBookId { get; set; }
    
    public int UserId { get; set; }
    
    [ForeignKey(nameof(EBookId))] public virtual EBook EBook { get; set; }
    
    [ForeignKey(nameof(UserId))] public virtual User User { get; set; }
}