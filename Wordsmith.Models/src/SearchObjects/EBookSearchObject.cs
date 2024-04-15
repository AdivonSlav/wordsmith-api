using Swashbuckle.AspNetCore.Annotations;

namespace Wordsmith.Models.SearchObjects;

public class EBookSearchObject : SearchObject
{
    [SwaggerParameter("Title of the ebook")]
    public string? Title { get; set; }
    
    [SwaggerParameter("List of genres")]
    public List<int>? Genres { get; set; }
    
    [SwaggerParameter("Maturity rating of the ebook")]
    public int? MaturityRatingId { get; set; }
    
    [SwaggerParameter("The ID of the book's author")]
    public int? AuthorId { get; set; }
}