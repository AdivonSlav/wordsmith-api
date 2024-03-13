using System.Text.Json.Serialization;

namespace Wordsmith.Integration.PayPal.Models;

public class PayPalDisplayData
{
    [JsonPropertyName("brand_name")]
    public string BrandName { get; set; }
}