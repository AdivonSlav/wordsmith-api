using System.Text.Json.Serialization;
using Wordsmith.Integration.Paypal.Enums;

namespace Wordsmith.Integration.Paypal.Models;

public class PaypalOrderRequest
{
    [JsonPropertyName("intent")]
    public PaypalIntent Intent { get; set; }
    
    [JsonPropertyName("purchase_units")]
    public IEnumerable<PaypalPurchaseUnit> PurchaseUnits { get; set; }
    
    [JsonPropertyName("payment_source")]
    public PaypalPaymentSource PaymentSource { get; set; }
}