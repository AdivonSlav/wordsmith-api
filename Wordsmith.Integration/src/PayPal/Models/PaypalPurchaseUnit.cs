using System.Text.Json.Serialization;

namespace Wordsmith.Integration.PayPal.Models;

public class PaypalPurchaseUnit
{
    [JsonPropertyName("reference_id")]
    public string ReferenceId { get; set; }
    
    [JsonPropertyName("amount")]
    public PayPalAmount Amount { get; set; }
    
    [JsonPropertyName("payee")]
    public PayPalPayee? Payee { get; set; }
    
    [JsonPropertyName("items")]
    public IEnumerable<PayPalItem> Items { get; set; }
}