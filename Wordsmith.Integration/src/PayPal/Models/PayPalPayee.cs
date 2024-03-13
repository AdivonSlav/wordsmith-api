using System.Text.Json.Serialization;

namespace Wordsmith.Integration.PayPal.Models;

public class PayPalPayee
{
    [JsonPropertyName("email_address")]
    public string EmailAddress { get; set; }
    
    [JsonPropertyName("merchant_id")]
    public string MerchantId { get; set; }
    
    [JsonPropertyName("display_data")]
    public PayPalDisplayData DisplayData { get; set; }
}