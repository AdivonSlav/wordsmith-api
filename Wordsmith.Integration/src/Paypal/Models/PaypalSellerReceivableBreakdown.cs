using System.Text.Json.Serialization;

namespace Wordsmith.Integration.Paypal.Models;

public class PaypalSellerReceivableBreakdown
{
    [JsonPropertyName("gross_amount")]
    public PaypalAmount GrossAmount { get; set; }
    
    [JsonPropertyName("paypal_fee")]
    public PaypalAmount PaypalFee { get; set; }
    
    [JsonPropertyName("net_amount")]
    public PaypalAmount NetAmount { get; set; }
}