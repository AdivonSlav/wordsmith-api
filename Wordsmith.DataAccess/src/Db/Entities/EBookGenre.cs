using System.ComponentModel.DataAnnotations.Schema;

namespace Wordsmith.DataAccess.Db.Entities;

[Table("ebook_genres")]
public class EBookGenre
{
    public int EBookId { get; set; }
    
    public int GenreId { get; set; }
    
    [ForeignKey(nameof(EBookId))]
    public virtual EBook EBook { get; set; }
    
    [ForeignKey(nameof(GenreId))]
    public virtual Genre Genre { get; set; }
}