using System.Text.Json.Serialization;

namespace Wordsmith.Integration.Paypal.Models;

public class PaypalPayer
{
    [JsonPropertyName("name")]
    public PaypalName Name { get; set; }
    
    [JsonPropertyName("email_address")]
    public string EmailAddress { get; set; }
    
    [JsonPropertyName("payer_id")]
    public string PayerId { get; set; }
}