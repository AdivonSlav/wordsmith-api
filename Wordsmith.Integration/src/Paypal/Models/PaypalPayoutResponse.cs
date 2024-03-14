using System.Text.Json.Serialization;

namespace Wordsmith.Integration.Paypal.Models;

public class PaypalPayoutResponse
{
    [JsonPropertyName("batch_header")]
    public PaypalBatchHeader BatchHeader { get; set; }
    
    [JsonPropertyName("links")]
    public IEnumerable<PaypalLink> Links { get; set; }
}