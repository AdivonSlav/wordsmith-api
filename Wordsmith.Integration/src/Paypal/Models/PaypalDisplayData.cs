using System.Text.Json.Serialization;

namespace Wordsmith.Integration.Paypal.Models;

public class PaypalDisplayData
{
    [JsonPropertyName("brand_name")]
    public string BrandName { get; set; }
}