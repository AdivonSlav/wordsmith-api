using System.Text.Json.Serialization;
using Wordsmith.Utils.JsonConversion;

namespace Wordsmith.Integration.Paypal.Enums;

[JsonConverter(typeof(ConstantCaseEnumConverter<PaypalRecipientWallet>))]
public enum PaypalRecipientWallet
{
    Paypal,
    RecipientSelected
}