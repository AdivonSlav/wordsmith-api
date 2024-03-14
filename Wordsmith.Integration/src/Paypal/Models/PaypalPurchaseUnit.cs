using System.Text.Json.Serialization;

namespace Wordsmith.Integration.Paypal.Models;

public class PaypalPurchaseUnit
{
    [JsonPropertyName("reference_id")]
    public string ReferenceId { get; set; }
    
    [JsonPropertyName("amount")]
    public PaypalAmount Amount { get; set; }
    
    [JsonPropertyName("payee")]
    public PaypalPayee Payee { get; set; }
    
    [JsonPropertyName("description")]
    public string Description { get; set; }
    
    [JsonPropertyName("items")]
    public IEnumerable<PaypalItem> Items { get; set; }
    
    [JsonPropertyName("payments")]
    public PaypalPayment Payments { get; set; }
}