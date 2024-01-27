namespace Wordsmith.Models.DataTransferObjects;

public class EBookDto
{
    public int Id { get; set; }
    
    public string Title { get; set; }

    public string Description { get; set; }

    public double? RatingAverage { get; set; }
    
    public DateTime PublishedDate { get; set; }
    
    public DateTime UpdatedDate { get; set; }
    
    public decimal? Price { get; set; }
    
    public int ChapterCount { get; set; }
    
    public string Path { get; set; }
    
    public ImageDto CoverArt { get; set; }
    
    public string Genres { get; set; }
    
    public MaturityRatingDto MaturityRating { get; set; }
}