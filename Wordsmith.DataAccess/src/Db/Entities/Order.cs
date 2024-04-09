using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Wordsmith.Models.Enums;

// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace Wordsmith.DataAccess.Db.Entities;

[Table("orders")]
[Index(nameof(ReferenceId))]
public class Order : IEntity
{
    [Key] public int Id { get; set; }
    
    [StringLength(50)]
    public string ReferenceId { get; set; }
    
    [StringLength(50)]
    public string PayPalOrderId { get; set; }
    
    [StringLength(50)]
    public string PayPalCaptureId { get; set; }
    
    [StringLength(50)]
    public string PayPalRefundId { get; set; }
    
    [StringLength(40)]
    public OrderStatus Status { get; set; }
    
    public int? PayerId { get; set; }
    
    [StringLength(20)]
    public string PayerUsername { get; set; }
    
    public int? PayeeId { get; set; }
    
    [StringLength(20)]
    public string PayeeUsername { get; set; }
    
    [StringLength(60)]
    public string PayeePayPalEmail { get; set; }
    
    public int? EBookId { get; set; }
    
    [StringLength(40)]
    public string EBookTitle { get; set; }
    
    public DateTime OrderCreationDate { get; set; }
    
    public DateTime? PaymentDate { get; set; }
    
    public DateTime? RefundDate { get; set; }
    
    [Precision(10, 2)]
    public decimal PaymentAmount { get; set; }
    
    [StringLength(100)]
    public string PaymentUrl { get; set; }

    [ForeignKey(nameof(PayerId))]
    [DeleteBehavior(DeleteBehavior.SetNull)]
    public virtual User Payer { get; set; }
    
    [ForeignKey(nameof(PayeeId))]
    [DeleteBehavior(DeleteBehavior.SetNull)]
    public virtual User Payee { get; set; }
    
    [ForeignKey(nameof(EBookId))]
    [DeleteBehavior(DeleteBehavior.SetNull)]
    public virtual EBook EBook { get; set; }
}