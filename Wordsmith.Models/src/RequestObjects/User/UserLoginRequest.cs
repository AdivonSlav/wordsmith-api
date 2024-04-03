using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Wordsmith.Models.RequestObjects.User;

public class UserLoginRequest
{
    [Required]
    [SwaggerSchema("Username encoded as a Base64 string")]
    public string Username { get; set; }
    
    [Required]
    [SwaggerSchema("Password encoded as a Base64 string")]
    public string Password { get; set; }
    
    public string? ClientId { get; set; }
}