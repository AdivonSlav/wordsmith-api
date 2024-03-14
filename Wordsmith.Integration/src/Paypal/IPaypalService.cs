using Wordsmith.Integration.Paypal.Models;

namespace Wordsmith.Integration.Paypal;

public interface IPaypalService
{
    public Task<PaypalOrderResponse> CreateOrder(decimal amount, string name, string description);
    public Task<PaypalCapturePaymentResponse> CapturePayment(string payPalOrderId);
    public Task<PaypalPayoutResponse> CreatePayout(string amount, string payeeEmail, string payerUsername,
        string ebookTitle);
    public Task<PaypalRefundResponse> RefundPayment(string payPalCaptureId);
}