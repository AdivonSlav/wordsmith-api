using System.Text.Json.Serialization;
using Wordsmith.Integration.Paypal.Enums;

namespace Wordsmith.Integration.Paypal.Models;

public class PaypalCapture
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("status")]
    public PaypalPaymentStatus Status { get; set; }
    
    [JsonPropertyName("amount")]
    public PaypalAmount Amount { get; set; }
    
    [JsonPropertyName("seller_receivable_breakdown")]
    public PaypalSellerReceivableBreakdown PaypalSellerReceivableBreakdown { get; set; }
    
    [JsonPropertyName("create_time")]
    public DateTime CreateTime { get; set; }
    
    [JsonPropertyName("update_time")]
    public DateTime UpdateTime { get; set; }
    
    [JsonPropertyName("links")]
    public IEnumerable<PaypalLink> Links { get; set; }
}