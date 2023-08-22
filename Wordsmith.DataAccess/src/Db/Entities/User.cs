using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wordsmith.DataAccess.Db.Entities;

[Table("users")]
public class User
{
    [Key] public int Id { get; set; }
    
    public string Username { get; set; }
    
    public string Email { get; set; }
    
    public int? ProfileImageId { get; set; }
    
    [ForeignKey("ProfileImageId")] public virtual Image ProfileImage { get; set; }
    
    public DateTime RegistrationDate { get; set; }
}