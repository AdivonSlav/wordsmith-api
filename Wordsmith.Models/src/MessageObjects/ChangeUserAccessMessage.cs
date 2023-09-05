namespace Wordsmith.Models.MessageObjects;

public class ChangeUserAccessMessage
{
    public int Id { get; set; }
    
    public bool AllowedAccess { get; set; }
    
    public DateTime? ExpiryDate { get; set; }
}