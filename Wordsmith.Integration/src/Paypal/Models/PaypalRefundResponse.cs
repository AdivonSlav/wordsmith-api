using System.Text.Json.Serialization;
using Wordsmith.Integration.Paypal.Enums;

namespace Wordsmith.Integration.Paypal.Models;

public class PaypalRefundResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("amount")]
    public PaypalAmount Amount { get; set; }
    
    [JsonPropertyName("status")]
    public PaypalRefundStatus Status { get; set; }

    [JsonPropertyName("links")]
    public IEnumerable<PaypalLink> Links { get; set; }
}
