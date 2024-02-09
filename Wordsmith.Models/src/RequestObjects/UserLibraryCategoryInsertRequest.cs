using System.ComponentModel.DataAnnotations;

namespace Wordsmith.Models.RequestObjects;

public class UserLibraryCategoryInsertRequest
{
    [Required]
    public int UserId { get; set; }
    
    [Required]
    [StringLength(maximumLength: 255)]
    public string Name { get; set; }
}