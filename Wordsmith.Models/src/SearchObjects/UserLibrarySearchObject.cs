using Swashbuckle.AspNetCore.Annotations;

namespace Wordsmith.Models.SearchObjects;

public class UserLibrarySearchObject : SearchObject
{
    [SwaggerParameter("The ID of the user that owns the library")]
    public int? UserId { get; set; }
    
    [SwaggerParameter("The ID of the specific ebook")]
    public int? EBookId { get; set; }
    
    [SwaggerParameter("Whether the user has read the book")]
    public bool? IsRead { get; set; }    
    
    [SwaggerParameter("Maturity rating ID of the book")]
    public int? MaturityRatingId { get; set; }
    
    [SwaggerParameter("The ID of the category that this entry belongs to")]
    public int? UserLibraryCategoryId { get; set; }
}