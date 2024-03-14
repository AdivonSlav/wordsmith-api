using System.Text.Json.Serialization;

namespace Wordsmith.Integration.Paypal.Models;

public class PaypalPaymentSource
{
    [JsonPropertyName("paypal")]
    public PaypalPaymentType Paypal { get; set; }
}