using System.Text.Json.Serialization;

namespace Wordsmith.Integration.Paypal.Models;

public class PaypalPayee
{
    [JsonPropertyName("email_address")]
    public string EmailAddress { get; set; }
    
    [JsonPropertyName("merchant_id")]
    public string MerchantId { get; set; }
    
    [JsonPropertyName("display_data")]
    public PaypalDisplayData DisplayData { get; set; }
}