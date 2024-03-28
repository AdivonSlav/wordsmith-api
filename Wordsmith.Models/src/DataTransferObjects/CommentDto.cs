using Swashbuckle.AspNetCore.Annotations;

namespace Wordsmith.Models.DataTransferObjects;

public class CommentDto
{
    [SwaggerSchema("ID of the comment")]
    public int Id { get; set; }
    
    [SwaggerSchema("The comment content")]
    public string Content { get; set; }
    
    [SwaggerSchema("Date when the comment was made")]
    public DateTime DateAdded { get; set; }
    
    [SwaggerSchema("Whether the comment is hidden or not")]
    public bool IsShown { get; set; }
    
    [SwaggerSchema("The ID of the chapter this comment refers to, if any")]
    public int? EBookChapterId { get; set; }
    
    [SwaggerSchema("The ebook that this comment refers to")]
    public int EBookId { get; set; }
    
    [SwaggerSchema("ID of the user who made the comment")]
    public int UserId { get; set; }
    
    [SwaggerSchema("User who made the comment")]
    public UserDto User { get; set; } 
    
    [SwaggerSchema("Whether the user who requested this response liked the comment or not")]
    public bool HasLiked { get; set; }

    [SwaggerSchema("The number of likes the comment has")]
    public long LikeCount { get; set; }
}