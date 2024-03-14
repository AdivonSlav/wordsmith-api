namespace Wordsmith.Models.DataTransferObjects;

public class OrderDto
{
    public int Id { get; set; }
    
    public string ReferenceId { get; set; }
    
    public string PayPalOrderId { get; set; }
    
    public string PayPalCaptureId { get; set; }
    
    public string PayPalRefundId { get; set; }
    
    public string Status { get; set; }
    
    public int? PayerId { get; set; }
    
    public string PayerUsername { get; set; }
    
    public int? PayeeId { get; set; }
    
    public string PayeeUsername { get; set; }
    
    public string PayeePayPalEmail { get; set; }
    
    public int? EBookId { get; set; }
    
    public string EBookTitle { get; set; }
    
    public DateTime OrderCreationDate { get; set; }
    
    public DateTime? PaymentDate { get; set; }
    
    public DateTime? RefundDate { get; set; }
    
    public decimal PaymentAmount { get; set; }
    
    public string PaymentUrl { get; set; }
}