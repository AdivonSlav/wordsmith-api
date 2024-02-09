using System.ComponentModel.DataAnnotations;

namespace Wordsmith.Models.RequestObjects.User;

public class UserChangeAccessRequest
{
    [Required]
    public bool AllowedAccess { get; set; }
    
    public DateTime? ExpiryDate { get; set; }
}