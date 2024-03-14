using System.Text.Json.Serialization;
using Wordsmith.Utils.JsonConversion;

namespace Wordsmith.Integration.Paypal.Enums;

[JsonConverter(typeof(ConstantCaseEnumConverter<PaypalPaymentStatus>))]
public enum PaypalPaymentStatus
{
    Completed,
    Declined,
    PartiallyRefunded,
    Pending,
    Refunded,
    Failed,
}