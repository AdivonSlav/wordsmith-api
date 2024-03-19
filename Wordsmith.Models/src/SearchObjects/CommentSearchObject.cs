using Swashbuckle.AspNetCore.Annotations;

namespace Wordsmith.Models.SearchObjects;

public class CommentSearchObject : SearchObject
{
    [SwaggerParameter("Whether the comment is visible or not")]
    public bool? IsShown { get; set; }
    
    [SwaggerParameter("The ID of the ebook chapter")]
    public int? EBookChapterId { get; set; }
    
    [SwaggerParameter("The ID of the ebook")]
    public int? EBookId { get; set; }
    
    [SwaggerParameter("The ID of the user that made the comment")]
    public int? UserId { get; set; }
}