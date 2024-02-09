using System.ComponentModel.DataAnnotations;
using Wordsmith.Models.RequestObjects.Image;
using Wordsmith.Models.ValidationAttributes;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Wordsmith.Models.RequestObjects.User;

public class UserInsertRequest
{
    [Required]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 12 characters")]
    public string Username { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [StringLength(20, MinimumLength = 6, ErrorMessage = "Password length must be between 6 and 20 characters")]
    [Password(ErrorMessage = "Password must have at least one digit and one special character")]
    public string Password { get; set; }
    
    [Required]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; }
    
    public ImageInsertRequest? ProfileImage { get; set; }
}