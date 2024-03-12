using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

#pragma warning disable CA1825

namespace Wordsmith.DataAccess.Db.Entities;

[Table("users")]
[Index(nameof(Username), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]
public class User : IEntity
{
    [Key] public int Id { get; set; }
    
    [StringLength(20)]
    public string Username { get; set; }
    
    [StringLength(100)]
    public string Email { get; set; }
    
    [StringLength(100)]
    public string About { get; set; }

    public DateTime RegistrationDate { get; set; }
    
    [StringLength(20)]
    public string Role { get; set; }
    
    public int? ProfileImageId { get; set; }
    
    [StringLength(100)]
    public string PayPalEmail { get; set; }
    
    [ForeignKey(nameof(ProfileImageId))] public virtual Image ProfileImage { get; set; }
}
