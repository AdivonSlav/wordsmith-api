using System.ComponentModel;
using System.Text.Json.Serialization;
using Wordsmith.Utils.JsonConversion;

namespace Wordsmith.Integration.Paypal.Enums;

[JsonConverter(typeof(ConstantCaseEnumConverter<PaypalCaptureStatusReason>))]
public enum PaypalCaptureStatusReason
{
    [Description("The buyer has initiated a dispute for this captured payment")]
    BuyerComplaint,
    
    [Description("The funds were reversed in response to a buyer-initiated dispute")]
    Chargeback,
    
    [Description("The buyer paid with an eCheck that has not yet cleared")]
    Echeck,
    
    [Description("Needs manual action")]
    InternationalWithdrawal,
    
    [Description("No additional reason can be provided. Contact PayPal")]
    Other,
    
    [Description("The captured payment is pending manual review")]
    PendingReview,
    
    [Description("The buyer has not yet set up appropriate receiving preferences for their PayPal account")]
    ReceivingPreferenceMandatesManualAction,
    
    [Description("The captured funds were refunded")]
    Refunded,
    
    [Description("The buyer must send the funds for this captured payment")]
    TransactionApprovedAwaitingFunding,
    
    [Description("The buyer does not have a PayPal account")]
    Unilateral,
    
    [Description("The buyer's PayPal account is not verified")]
    VerificationRequired
}