using System.Text.Json.Serialization;
using Wordsmith.Utils.JsonConversion;

namespace Wordsmith.Integration.Paypal.Enums;

[JsonConverter(typeof(ConstantCaseEnumConverter<PaypalOrderStatus>))]
public enum PaypalOrderStatus
{
    Created,
    Saved,
    Approved,
    Voided,
    Completed,
    PayerActionRequired,
}