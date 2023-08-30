using System.ComponentModel.DataAnnotations;

namespace Wordsmith.Models.RequestObjects;

public class UserUpdateRequest
{
    [StringLength(12, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 12 characters")]
    public string? Username { get; set; }
    
    [EmailAddress]
    public string? Email { get; set; }
    
    [StringLength(20, MinimumLength = 6, ErrorMessage = "Password length must be between 6 and 20 characters")]
    public string? Password { get; set; }
    
    public string? OldPassword { get; set; }
    
    public ImageInsertRequest? ProfileImage { get; set; }
}