namespace Wordsmith.Models.SearchObjects;

public class EBookSearchObject : SearchObject
{
    public string? Title { get; set; }
    
    public List<int>? Genres { get; set; }
    
    public int? MaturityRatingId { get; set; }
}