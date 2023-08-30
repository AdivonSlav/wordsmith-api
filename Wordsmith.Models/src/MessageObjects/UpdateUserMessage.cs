namespace Wordsmith.Models.MessageObjects;

public class UpdateUserMessage
{
    public int Id { get; set; }
    
    public string? Username { get; set; }
    
    public string? Email { get; set; }
    
    public string? Password { get; set; }
    
    public string? OldPassword { get; set; }
    
    public string? Role { get; set; }
}