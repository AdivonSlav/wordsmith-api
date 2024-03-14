using System.Text.Json.Serialization;

namespace Wordsmith.Integration.Paypal.Models;

public class PaypalBreakdown
{
    [JsonPropertyName("item_total")]
    public PaypalItemTotal ItemTotal { get; set; }
}