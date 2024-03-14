using System.Text.Json.Serialization;

namespace Wordsmith.Integration.Paypal.Models;

public class PaypalPaymentType
{
    [JsonPropertyName("experience_context")]
    public PaypalExperienceContext ExperienceContext { get; set; }
    
    [JsonPropertyName("name")]
    public PaypalName Name { get; set; }
    
    [JsonPropertyName("email_address")]
    public string EmailAddress { get; set; }
    
    [JsonPropertyName("account_id")]
    public string AccountId { get; set; }
}