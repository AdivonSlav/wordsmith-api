using System.Text.Json.Serialization;

namespace Wordsmith.Integration.Paypal.Models;

public class PaypalOrderResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("intent")]
    public string Intent { get; set; }
    
    [JsonPropertyName("status")]
    public string Status { get; set; }
    
    [JsonPropertyName("purchase_units")]
    public IEnumerable<PaypalPurchaseUnit> PurchaseUnits { get; set; }
    
    [JsonPropertyName("links")]
    public IEnumerable<PaypalLink> Links { get; set; }
}