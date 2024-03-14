using System.Text.Json.Serialization;
using Wordsmith.Utils.JsonConversion;

namespace Wordsmith.Integration.Paypal.Enums;

[JsonConverter(typeof(ConstantCaseEnumConverter<PaypalRecipientType>))]
public enum PaypalRecipientType
{
    Email,
    Phone,
    PaypalId
}