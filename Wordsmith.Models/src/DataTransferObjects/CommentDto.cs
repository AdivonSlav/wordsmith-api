namespace Wordsmith.Models.DataTransferObjects;

public class CommentDto
{
    public int Id { get; set; }
    
    public string Content { get; set; }
    
    public DateTime DateAdded { get; set; }
    
    public bool IsShown { get; set; }
    
    public int? EBookChapterId { get; set; }
    
    public int EBookId { get; set; }
    
    public int UserId { get; set; }
}