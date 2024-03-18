using System.ComponentModel.DataAnnotations;

namespace Wordsmith.Models.RequestObjects.UserLibrary;

public class UserLibraryInsertRequest
{
    public int? UserId { get; set; }
    
    [Required]
    public int EBookId { get; set; }
    
    public string? OrderReferenceId { get; set; }
}