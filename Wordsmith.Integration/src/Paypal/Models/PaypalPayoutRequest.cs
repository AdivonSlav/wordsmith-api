using System.Text.Json.Serialization;

namespace Wordsmith.Integration.Paypal.Models;

public class PaypalPayoutRequest
{
    [JsonPropertyName("sender_batch_header")]
    public PaypalSenderBatchHeader SenderBatchHeader { get; set; }
    
    [JsonPropertyName("items")]
    public IEnumerable<PaypalPayoutItem> Items { get; set; }
}