using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#pragma warning disable CA1825

namespace Wordsmith.DataAccess.Db.Entities;

[Table("users")]
[Index(nameof(Username), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]
public class User
{
    [Key] public int Id { get; set; }
    
    public string Username { get; set; }
    
    public string Email { get; set; }
    
    public string About { get; set; }

    public DateTime RegistrationDate { get; set; }
    
    public int? ProfileImageId { get; set; }
    
    public int RoleId { get; set; }
    
    [ForeignKey(nameof(ProfileImageId))] public virtual Image ProfileImage { get; set; }
    
    [ForeignKey(nameof(RoleId))] public virtual Role Role { get; set; }
}
