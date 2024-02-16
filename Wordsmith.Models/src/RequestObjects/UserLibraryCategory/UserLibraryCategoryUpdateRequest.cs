using System.ComponentModel.DataAnnotations;

namespace Wordsmith.Models.RequestObjects.UserLibraryCategory;

public class UserLibraryCategoryUpdateRequest
{
    [StringLength(255, MinimumLength = 1, ErrorMessage = "Category name must not exceed 255 characters!")]
    public string? Name { get; set; }
}