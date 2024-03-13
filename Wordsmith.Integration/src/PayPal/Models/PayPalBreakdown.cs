using System.Text.Json.Serialization;

namespace Wordsmith.Integration.PayPal.Models;

public class PayPalBreakdown
{
    [JsonPropertyName("item_total")]
    public PayPalItemTotal ItemTotal { get; set; }
}