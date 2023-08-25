using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#pragma warning disable CA1825

namespace Wordsmith.DataAccess.Db.Entities;

[Table("ebook_promotions")]
[Index(nameof(IsInProgress))]
public class EBookPromotion
{
    [Key] public int Id { get; set; }
    
    public DateTime PromotionStart { get; set; }
    
    public DateTime PromotionEnd { get; set; }
    
    public int PromotionLength { get; set; }
    
    public bool IsInProgress { get; set; }
    
    public int UserId { get; set; }
    
    public int EBookId { get; set; }
    
    [ForeignKey(nameof(UserId))] public virtual User Admin { get; set; }
    
    [ForeignKey(nameof(EBookId))] public virtual EBook EBook { get; set; }
}