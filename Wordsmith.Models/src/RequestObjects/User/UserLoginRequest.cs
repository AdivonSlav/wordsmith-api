using System.ComponentModel.DataAnnotations;

namespace Wordsmith.Models.RequestObjects.User;

public class UserLoginRequest
{
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string Password { get; set; }
}