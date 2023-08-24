namespace Wordsmith.Models.MessageObjects;

public class RegisterUserMessage
{
    public int Id { get; set; }
    
    public string Username { get; set; }
    
    public string Email { get; set; }
    
    public string Password { get; set; }
}