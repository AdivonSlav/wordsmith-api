using System.Text.Json.Serialization;

namespace Wordsmith.Integration.Paypal.Models;

public class PaypalUnitAmount
{
    [JsonPropertyName("currency_code")]
    public string CurrencyCode { get; set; }
    
    [JsonPropertyName("value")]
    public string Value { get; set; }
}