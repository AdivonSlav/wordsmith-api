using System.ComponentModel.DataAnnotations;

namespace Wordsmith.Models.RequestObjects;

public class UserLibraryInsertRequest
{
    [Required]
    public int UserId { get; set; }
    
    [Required]
    public int EBookId { get; set; }
}