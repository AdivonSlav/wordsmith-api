using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wordsmith.DataAccess.Db.Entities;

[Table("images")]
public class Image : IEntity
{
    [Key] public int Id { get; set; }
    
    public string Path { get; set; }
    
    public string Format { get; set; }
    
    public int Size { get; set; }
}