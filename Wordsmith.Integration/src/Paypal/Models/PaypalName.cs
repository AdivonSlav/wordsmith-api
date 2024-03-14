using System.Text.Json.Serialization;

namespace Wordsmith.Integration.Paypal.Models;

public class PaypalName
{
    [JsonPropertyName("given_name")]
    public string GivenName { get; set; }
    
    [JsonPropertyName("surname")]
    public string Surname { get; set; }
}