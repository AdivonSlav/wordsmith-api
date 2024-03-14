using System.Text.Json.Serialization;
using Wordsmith.Integration.Paypal.Enums;

namespace Wordsmith.Integration.Paypal.Models;

public class PaypalCaptureStatusDetails
{
    [JsonPropertyName("reason")]
    public PaypalCaptureStatusReason Reason { get; set; }
}