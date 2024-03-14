using System.Text.Json.Serialization;

namespace Wordsmith.Integration.Paypal.Models;

public class PaypalAmount
{
    [JsonPropertyName("currency_code")]
    public string CurrencyCode { get; set; }
    
    [JsonPropertyName("currency")]
    public string Currency { get; set; }
    
    [JsonPropertyName("value")]
    public string Value { get; set; }
    
    [JsonPropertyName("breakdown")]
    public PaypalBreakdown Breakdown { get; set; }
}