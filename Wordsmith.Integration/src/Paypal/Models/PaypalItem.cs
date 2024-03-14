using System.Text.Json.Serialization;

namespace Wordsmith.Integration.Paypal.Models;

public class PaypalItem
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("description")]
    public string Description { get; set; }
    
    [JsonPropertyName("quantity")]
    public string Quantity { get; set; }
    
    [JsonPropertyName("unit_amount")]
    public PaypalUnitAmount UnitAmount { get; set; }
}