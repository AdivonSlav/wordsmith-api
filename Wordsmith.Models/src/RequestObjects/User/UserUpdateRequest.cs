using System.ComponentModel.DataAnnotations;
using Wordsmith.Models.ValidationAttributes;

namespace Wordsmith.Models.RequestObjects.User;

public class UserUpdateRequest
{
    [StringLength(20, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 12 characters")]
    public string Username { get; set; }
    
    [EmailAddress]
    public string Email { get; set; }
    
    [StringLength(100, ErrorMessage = "The about me information can be a maximum of 100 characters")]
    public string About { get; set; }
    
    [StringLength(20, MinimumLength = 6, ErrorMessage = "Password length must be between 6 and 20 characters")]
    [Password(ErrorMessage = "Password must have at least one digit and one special character")]
    public string? Password { get; set; }
    
    [RequiredIfOtherPresent(nameof(Password))]
    public string? OldPassword { get; set; }
}