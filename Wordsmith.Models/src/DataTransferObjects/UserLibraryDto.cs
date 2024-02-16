namespace Wordsmith.Models.DataTransferObjects;

public class UserLibraryDto
{
    public int Id { get; set; }
    
    public int EBookId { get; set; }
    
    public int UserId { get; set; }
    
    public EBookDto? EBook { get; set; }
    
    public DateTime SyncDate { get; set; }
    
    public bool IsRead { get; set; }
    
    public string ReadProgress { get; set; }
    
    public int? UserLibraryCategoryId { get; set; }
    
    public int LastChapterId { get; set; }
    
    public int LastPage { get; set; }
}