namespace Wordsmith.Models.SearchObjects;

public class EBookSearchObject : SearchObject
{
    public string? Title { get; set; }
    
    public double? RatingAverage { get; set; }
    
    public DateTime? PublishedDate { get; set; }
    
    public DateTime? UpdatedDate { get; set; }
    
    public decimal? Price { get; set; }
    
    public int? GenreId { get; set; }
    
    public int? MaturityRatingId { get; set; }
}