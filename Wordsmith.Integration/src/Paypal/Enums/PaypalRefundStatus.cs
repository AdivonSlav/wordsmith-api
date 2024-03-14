using System.ComponentModel;
using System.Text.Json.Serialization;
using Wordsmith.Utils.JsonConversion;

namespace Wordsmith.Integration.Paypal.Enums;

[JsonConverter(typeof(ConstantCaseEnumConverter<PaypalRefundStatus>))]
public enum PaypalRefundStatus
{
    [Description("The refund was cancelled")]
    Cancelled,
    
    [Description("The refund could not be processed")]
    Failed,
    
    [Description("The refund is pending")]
    Pending,
    
    [Description("The refund is complete")]
    Completed
}