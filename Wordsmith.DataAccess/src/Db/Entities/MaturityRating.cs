using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#pragma warning disable CA1825

namespace Wordsmith.DataAccess.Db.Entities;

[Table("maturity_ratings")]
[Index(nameof(Name), IsUnique = true)]
public class MaturityRating : IEntity
{
    [Key] public int Id { get; set; }
    
    [StringLength(40)]
    public string Name { get; set; }
    
    [StringLength(5)]
    public string ShortName { get; set; }
}