using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace Wordsmith.DataAccess.Db.Entities;

[Table("ebook_genres")]
public class EBookGenre : IEntity
{
    [Key] public int Id { get; set; }
    
    public int EBookId { get; set; }
    
    public int GenreId { get; set; }
    
    [ForeignKey(nameof(EBookId))]
    public virtual EBook EBook { get; set; }
    
    [ForeignKey(nameof(GenreId))]
    public virtual Genre Genre { get; set; }
}