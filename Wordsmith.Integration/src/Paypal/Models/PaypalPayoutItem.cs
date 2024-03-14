using System.Text.Json.Serialization;
using Wordsmith.Integration.Paypal.Enums;

namespace Wordsmith.Integration.Paypal.Models;

public class PaypalPayoutItem
{
    [JsonPropertyName("amount")]
    public PaypalAmount Amount { get; set; }
    
    [JsonPropertyName("note")]
    public string Note { get; set; }
    
    [JsonPropertyName("receiver")]
    public string Receiver { get; set; }
    
    [JsonPropertyName("recipient_wallet")]
    public PaypalRecipientWallet RecipientWallet { get; set; }
}