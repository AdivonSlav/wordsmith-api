namespace Wordsmith.Models.DataTransferObjects;

public class UserLoginDto
{
    public string? AccessToken { get; set; }
    
    public string? RefreshToken { get; set; }
    
    public int? ExpiresIn { get; set; }
    
    public UserDto User { get; set; }
}