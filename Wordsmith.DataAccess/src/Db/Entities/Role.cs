using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#pragma warning disable CA1825

namespace Wordsmith.DataAccess.Db.Entities;

[Table("roles")]
[Index(nameof(Name), IsUnique = true)]
public class Role
{
    [Key] public int Id { get; set; }
    
    public string Name { get; set; }
}