namespace Wordsmith.Models.DataTransferObjects;

public class UserDto
{
    public int Id { get; set; }
    
    public string Username { get; set; }
    
    public string Email { get; set; }
    
    public ImageDto? ProfileImage { get; set; }
    
    public DateTime RegistrationDate { get; set; }
    
    public string Status { get; set; }
}