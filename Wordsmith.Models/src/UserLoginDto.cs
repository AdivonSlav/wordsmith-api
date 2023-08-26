namespace Wordsmith.Models;

public class UserLoginDto
{
    public string? AccessToken { get; set; }
    
    public string? RefreshToken { get; set; }
    
    public int? ExpiresIn { get; set; }
}