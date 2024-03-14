using System.Text.Json.Serialization;

namespace Wordsmith.Integration.Paypal.Models;

public class PaypalPayment
{
    [JsonPropertyName("captures")]
    public IEnumerable<PaypalCapture> Captures { get; set; }
}