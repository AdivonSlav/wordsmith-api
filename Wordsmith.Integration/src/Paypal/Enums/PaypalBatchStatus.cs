using System.ComponentModel;
using System.Text.Json.Serialization;
using Wordsmith.Utils.JsonConversion;

namespace Wordsmith.Integration.Paypal.Enums;

[JsonConverter(typeof(ConstantCaseEnumConverter<PaypalBatchStatus>))]
public enum PaypalBatchStatus
{
    [Description("Payout request was denied")]
    Denied,
    
    [Description("Payout request was received and will be processed")]
    Pending,
    
    [Description("Payout request was received and is being processed")]
    Processing,
    
    [Description("Payout request is succesfull")]
    Success,
    
    [Description("Payout request was canceled")]
    Canceled
}