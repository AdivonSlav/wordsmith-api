namespace Wordsmith.Models.RequestObjects.UserLibrary;

public class UserLibraryUpdateRequest
{
    public bool? IsRead { get; set; }
    
    public string? ReadProgress { get; set; }
    
    public int? LastChapterId { get; set; }
    
    public int? LastPage { get; set; }
}