using System.Text.Json.Serialization;
using Wordsmith.Integration.Paypal.Enums;

namespace Wordsmith.Integration.Paypal.Models;

public class PaypalBatchHeader
{
    [JsonPropertyName("payout_batch_id")]
    public string PayoutBatchId { get; set; }
    
    [JsonPropertyName("time_created")]
    public DateTime TimeCreated { get; set; }
    
    [JsonPropertyName("batch_status")]
    public PaypalBatchStatus BatchStatus { get; set; }
    
    [JsonPropertyName("sender_batch_header")]
    public PaypalSenderBatchHeader SenderBatchHeader { get; set; }
}