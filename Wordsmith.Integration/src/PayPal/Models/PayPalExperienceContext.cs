using System.Text.Json.Serialization;

namespace Wordsmith.Integration.PayPal.Models;

public class PayPalExperienceContext
{
    [JsonPropertyName("payment_method_preference")]
    public string PaymentMethodPreference { get; set; }
    
    [JsonPropertyName("brand_name")]
    public string BrandName { get; set; }
    
    [JsonPropertyName("locale")]
    public string Locale { get; set; }
    
    [JsonPropertyName("landing_page")]
    public string LandingPage { get; set; }
    
    [JsonPropertyName("shipping_preference")]
    public string ShippingPreference { get; set; }
    
    [JsonPropertyName("user_action")]
    public string UserAction { get; set; }
    
    [JsonPropertyName("return_url")]
    public string ReturnUrl { get; set; }
    
    [JsonPropertyName("cancel_url")]
    public string CancelUrl { get; set; }
}