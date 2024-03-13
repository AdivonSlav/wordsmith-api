using System.Text.Json.Serialization;

namespace Wordsmith.Integration.PayPal.Models;

public class PayPalOrderResponse
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
    public IEnumerable<PayPalLink> Links { get; set; }
}