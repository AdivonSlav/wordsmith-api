using System.ComponentModel.DataAnnotations;

namespace Wordsmith.Models.RequestObjects;

public class UserChangeAccessRequest
{
    [Required]
    public bool AllowedAccess { get; set; }
    
    public DateTime? ExpiryDate { get; set; }
}