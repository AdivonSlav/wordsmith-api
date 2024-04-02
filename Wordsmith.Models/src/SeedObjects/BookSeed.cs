namespace Wordsmith.Models.SeedObjects;

public class BookSeed
{
    public string Title { get; set; }
    public string Description { get; set; }
    public double RatingAverage { get; set; }
    public decimal? Price { get; set; }
    public string Genres { get; set; }
    public string MaturityRating { get; set; }
    public string BookFilename { get; set; }
}