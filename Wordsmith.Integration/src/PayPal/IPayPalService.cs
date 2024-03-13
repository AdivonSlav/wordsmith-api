using Wordsmith.Integration.PayPal.Models;

namespace Wordsmith.Integration.PayPal;

public interface IPayPalService
{
    public Task<PayPalOrderResponse> CreateOrder(decimal amount, string name, string description);
}