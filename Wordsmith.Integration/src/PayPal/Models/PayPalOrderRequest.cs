using System.Text.Json.Serialization;

namespace Wordsmith.Integration.PayPal.Models;

public class PayPalOrderRequest
{
    [JsonPropertyName("intent")]
    public string Intent { get; set; }
    
    [JsonPropertyName("purchase_units")]
    public IEnumerable<PaypalPurchaseUnit> PurchaseUnits { get; set; }
    
    [JsonPropertyName("payment_source")]
    public PayPalPaymentSource PaymentSource { get; set; }
}