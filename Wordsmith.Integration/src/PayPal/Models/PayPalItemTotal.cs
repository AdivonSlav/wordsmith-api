using System.Text.Json.Serialization;

namespace Wordsmith.Integration.PayPal.Models;

public class PayPalItemTotal
{
    [JsonPropertyName("currency_code")]
    public string CurrencyCode { get; set; }
    
    [JsonPropertyName("value")]
    public string Value { get; set; }
}