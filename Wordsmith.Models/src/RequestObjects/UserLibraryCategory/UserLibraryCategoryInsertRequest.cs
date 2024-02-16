using System.ComponentModel.DataAnnotations;

namespace Wordsmith.Models.RequestObjects.UserLibraryCategory;

public class UserLibraryCategoryInsertRequest
{
    [Required]
    [MaxLength(length: 20, ErrorMessage = "You can only add a maximum of 20 entries to a category at a time")]
    public List<int> UserLibraryIds { get; set; }    
    
    public int UserId { get; set; }
    
    public int? UserLibraryCategoryId { get; set; }
    
    [StringLength(255, MinimumLength = 1, ErrorMessage = "Category name must not exceed 255 characters!")]
    public string? NewCategoryName { get; set; }
}