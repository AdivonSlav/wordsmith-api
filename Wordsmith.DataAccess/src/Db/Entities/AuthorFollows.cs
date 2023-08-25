using System.ComponentModel.DataAnnotations.Schema;

namespace Wordsmith.DataAccess.Db.Entities;

[Table("author_follows")]
public class AuthorFollow
{
    public int AuthorUserId { get; set; }
    
    public int UserId { get; set; }
    
    [ForeignKey(nameof(AuthorUserId))] public virtual User AuthorUser { get; set; }
    
    [ForeignKey(nameof(UserId))] public virtual User User { get; set; }
}