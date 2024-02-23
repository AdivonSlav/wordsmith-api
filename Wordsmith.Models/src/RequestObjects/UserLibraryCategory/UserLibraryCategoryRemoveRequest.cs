using System.ComponentModel.DataAnnotations;

namespace Wordsmith.Models.RequestObjects.UserLibraryCategory;

public class UserLibraryCategoryRemoveRequest
{
    [Required]
    public List<int> UserLibraryIds { get; set; }    
    
    public int UserId { get; set; }
}