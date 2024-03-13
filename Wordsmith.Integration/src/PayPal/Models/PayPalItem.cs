using System.Text.Json.Serialization;

namespace Wordsmith.Integration.PayPal.Models;

public class PayPalItem
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("description")]
    public string Description { get; set; }
    
    [JsonPropertyName("quantity")]
    public string Quantity { get; set; }
    
    [JsonPropertyName("unit_amount")]
    public PayPalUnitAmount UnitAmount { get; set; }
}