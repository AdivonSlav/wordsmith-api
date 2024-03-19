using System.ComponentModel.DataAnnotations;

namespace Wordsmith.Models.RequestObjects.Comment;

public class CommentUpdateRequest
{
    [Required]
    [StringLength(maximumLength: 400, MinimumLength = 5, ErrorMessage = "Comment content must be between 5 and 400 characters")]
    public string Content { get; set; }
    
    public int? EBookChapterId { get; set; }
    
    [Required]
    public int EBookId { get; set; }
    
    public int UserId { get; set; }
}