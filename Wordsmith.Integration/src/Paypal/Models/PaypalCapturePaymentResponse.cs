using System.Text.Json.Serialization;
using Wordsmith.Integration.Paypal.Enums;

namespace Wordsmith.Integration.Paypal.Models;

public class PaypalCapturePaymentResponse
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("intent")]
    public PaypalIntent Intent { get; set; }
    
    [JsonPropertyName("status")]
    public PaypalOrderStatus Status { get; set; }

    [JsonPropertyName("payment_source")]
    public PaypalPaymentSource PaymentSource { get; set; }
    
    [JsonPropertyName("purchase_units")]
    public IEnumerable<PaypalPurchaseUnit> PurchaseUnits { get; set; }
    
    [JsonPropertyName("payer")]
    public PaypalPayer Payer { get; set; }
    
    [JsonPropertyName("links")]
    public IEnumerable<PaypalLink> Links { get; set; }
}