using System.Text.Json.Serialization;

namespace Wordsmith.Integration.PayPal.Models;

public class PayPalPayment
{
    [JsonPropertyName("experience_context")]
    public PayPalExperienceContext ExperienceContext { get; set; }
}