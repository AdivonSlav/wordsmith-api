using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#pragma warning disable CA1825

namespace Wordsmith.DataAccess.Db.Entities;

[Table("ebook")]
[Index(nameof(Title))]
[Index(nameof(Price))]
[Index(nameof(PublishedDate))]
[Index(nameof(UpdatedDate))]
[Index(nameof(RatingAverage))]
public class EBook
{
    [Key] public int Id { get; set; }
    
    public string Title { get; set; }
    
    public string Description { get; set; }
    
    public double? RatingAverage { get; set; }
    
    public DateTime PublishedDate { get; set; }
    
    public DateTime UpdatedDate { get; set; }
    
    [Precision(10, 2)] public decimal? Price { get; set; }
    
    public int ChapterCount { get; set; }
    
    public string Path { get; set; }
    
    public bool IsHidden { get; set; }
    
    public DateTime? HiddenDate { get; set; }
    
    public int AuthorId { get; set; }
    
    public int CoverArtId { get; set; }
    
    public int GenreId { get; set; }
    
    public int MaturityRatingId { get; set; }
    
    [ForeignKey(nameof(AuthorId))] public virtual User Author { get; set; }
    
    [ForeignKey(nameof(CoverArtId))] public virtual Image CoverArt { get; set; }
    
    [ForeignKey(nameof(GenreId))] public virtual Genre Genre { get; set; }
    
    [ForeignKey(nameof(MaturityRatingId))] public virtual MaturityRating MaturityRating { get; set; }
}