using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wordsmith.DataAccess.Db.Entities;

[Table("comment_likes")]
public class CommentLike : IEntity
{
    [Key]
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    public int CommentId { get; set; }
    
    public DateTime LikeDate { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public virtual User User { get; set; }
    
    [ForeignKey(nameof(CommentId))]
    public virtual Comment Comment { get; set; }
}