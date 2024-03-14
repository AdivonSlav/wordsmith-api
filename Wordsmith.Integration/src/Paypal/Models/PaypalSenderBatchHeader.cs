using System.Text.Json.Serialization;
using Wordsmith.Integration.Paypal.Enums;

namespace Wordsmith.Integration.Paypal.Models;

public class PaypalSenderBatchHeader
{
    [JsonPropertyName("sender_batch_header")]
    public string SenderBatchId { get; set; }
    
    [JsonPropertyName("recipient_type")]
    public PaypalRecipientType RecipientType { get; set; }
    
    [JsonPropertyName("email_subject")]
    public string EmailSubject { get; set; }
    
    [JsonPropertyName("email_message")]
    public string EmailMessage { get; set; }
}