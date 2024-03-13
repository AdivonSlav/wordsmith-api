using System.Text.Json.Serialization;

namespace Wordsmith.Integration.PayPal.Models;

public class PayPalPaymentSource
{
    [JsonPropertyName("paypal")]
    public PayPalPayment Paypal { get; set; }
}