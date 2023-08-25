using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#pragma warning disable CA1825

namespace Wordsmith.DataAccess.Db.Entities;

[Table("ebook_sales")]
[Index(nameof(Price))]
[Index(nameof(PurchaseDate))]
public class EBookSale
{
    [Key] public int Id { get; set; }
    
    public DateTime PurchaseDate { get; set; }
    
    [Precision(10, 2)]
    public decimal Price { get; set; }
    
    public int EBookId { get; set; }
    
    public int UserId { get; set; }
    
    [ForeignKey(nameof(EBookId))] public virtual EBook  EBook { get; set; }
    
    [ForeignKey(nameof(UserId))] public virtual User User { get; set; }
}