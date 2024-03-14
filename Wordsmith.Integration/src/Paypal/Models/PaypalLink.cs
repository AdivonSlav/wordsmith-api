using System.Text.Json.Serialization;

namespace Wordsmith.Integration.Paypal.Models;

public class PaypalLink
{
    [JsonPropertyName("href")]
    public string Href { get; set; }
    
    [JsonPropertyName("rel")]
    public string Rel { get; set; }
    
    [JsonPropertyName("method")]
    public string Method { get; set; }
}