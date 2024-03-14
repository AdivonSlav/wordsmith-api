using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Wordsmith.Integration.Paypal.Enums;
using Wordsmith.Integration.Paypal.Models;
using Wordsmith.Utils;

namespace Wordsmith.Integration.Paypal;

public class PaypalService : IPaypalService
{
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly IHttpClientFactory _httpClientFactory;

    private const string PaypalAuthEndpoint = "https://api-m.sandbox.paypal.com/v1/oauth2/token";
    private const string PaypalOrdersEndpoint = "https://api-m.sandbox.paypal.com/v2/checkout/orders";
    private const string PaypalCapturePaymentEndpoint = "https://api-m.sandbox.paypal.com/v2/checkout/orders/{}/capture";
    private const string PaypalRefundPaymentEndpoint =
        "https://api-m.paypal.com/v2/payments/captures/{}/refund";
    private const string PaypalPayoutsEndpoint = "https://api-m.sandbox.paypal.com/v1/payments/payouts";
    
    private string _accessToken;
    private DateTime _accessTokenExpiration;

    public PaypalService(string clientId, string clientSecret, IHttpClientFactory httpClientFactory)
    {
        _clientId = clientId;
        _clientSecret = clientSecret;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<PaypalOrderResponse> CreateOrder(decimal amount, string name, string description)
    {
        var accessToken = await GetAccessToken();
        
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        httpClient.DefaultRequestHeaders.Add("Prefer", "return=representation");
        httpClient.DefaultRequestHeaders.Add("PayPal-Request-Id", Guid.NewGuid().ToString());

        var orderRequest = new PaypalOrderRequest()
        {
            Intent = PaypalIntent.Capture,
            PurchaseUnits = new List<PaypalPurchaseUnit>()
            {
                new()
                {
                    ReferenceId = Guid.NewGuid().ToString(),
                    Amount = new PaypalAmount()
                    {
                      CurrencyCode  = "USD",
                      Value = amount.ToString(CultureInfo.InvariantCulture),
                      Breakdown = new PaypalBreakdown()
                      {
                          ItemTotal = new PaypalItemTotal()
                          {
                              CurrencyCode = "USD",
                              Value = amount.ToString(CultureInfo.InvariantCulture),
                          }
                      },
                    },
                    Items = new List<PaypalItem>()
                    {
                        new()
                        {
                            Name = name,
                            Description = description,
                            Quantity = "1",
                            UnitAmount = new PaypalUnitAmount()
                            {
                                CurrencyCode = "USD",
                                Value = amount.ToString(CultureInfo.InvariantCulture),
                            }
                        }
                    },
                }
            },
            PaymentSource = new PaypalPaymentSource()
            {
                Paypal = new PaypalPaymentType()
                {
                    ExperienceContext = new PaypalExperienceContext()
                    {
                        PaymentMethodPreference = "IMMEDIATE_PAYMENT_REQUIRED",
                        BrandName = "Wordsmith",
                        Locale = "en-US",
                        LandingPage = "LOGIN",
                        ShippingPreference = "NO_SHIPPING",
                        UserAction = "PAY_NOW",
                        ReturnUrl = "https://example.com/returnUrl",
                        CancelUrl = "https://example.com/cancelUrl"
                    } 
                }
            }
        };

        var requestPayload = JsonSerializer.Serialize(orderRequest);
        var requestContent = new StringContent(requestPayload, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(PaypalOrdersEndpoint, requestContent);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Status {response.StatusCode} Failed to create PayPal order: {responseContent}");
        }
        
        var orderResponse = JsonSerializer.Deserialize<PaypalOrderResponse>(responseContent);

        if (orderResponse == null)
        {
            throw new Exception($"Could not deserialize PayPal order create response to {typeof(PaypalOrderResponse)}");
        }
        
        Logger.LogInfo($"Created PayPal order with reference ID {orderResponse.PurchaseUnits.First().ReferenceId}");
        return orderResponse;
    }

    public async Task<PaypalCapturePaymentResponse> CapturePayment(string payPalOrderId)
    {
        var accessToken = await GetAccessToken();
        
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        httpClient.DefaultRequestHeaders.Add("Prefer", "return=representation");
        httpClient.DefaultRequestHeaders.Add("PayPal-Request-Id", Guid.NewGuid().ToString());

        var requestContent = new StringContent(string.Empty);
        requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        requestContent.Headers.ContentLength = 0;
        
        var url = PaypalCapturePaymentEndpoint.Replace("{}", payPalOrderId);
        var response = await httpClient.PostAsync(url, requestContent);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(
                $"Status {response.StatusCode}: Failed to capture PayPal order with ID {payPalOrderId}: {responseContent}");
        }

        var captureResponse = JsonSerializer.Deserialize<PaypalCapturePaymentResponse>(responseContent);

        if (captureResponse == null)
        {
            throw new Exception($"Could not deserialize PayPal order capture response to {typeof(PaypalCapturePaymentResponse)}");
        }
        
        Logger.LogInfo($"Captured PayPal order with reference ID {captureResponse.PurchaseUnits.First().ReferenceId}");
        return captureResponse;
    }

    public async Task<PaypalPayoutResponse> CreatePayout(string amount, string payeeEmail, string payerUsername,
        string ebookTitle)
    {
        var accessToken = await GetAccessToken();

        var httpClient = _httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        httpClient.DefaultRequestHeaders.Add("PayPal-Request-Id", Guid.NewGuid().ToString());

        var payoutRequest = new PaypalPayoutRequest()
        {
            SenderBatchHeader = new PaypalSenderBatchHeader()
            {
                SenderBatchId = Guid.NewGuid().ToString(),
                EmailSubject = "New payment for ebook",
                EmailMessage = $"User {payerUsername} has purchased {ebookTitle}. Wordsmith collects payments from buyers and pays the author automatically upon order completion.",
                RecipientType = PaypalRecipientType.Email
            },
            Items = new []
            {
                new PaypalPayoutItem()
                {
                    Receiver = payeeEmail,
                    RecipientWallet = PaypalRecipientWallet.Paypal,
                    Amount = new PaypalAmount()
                    {
                        Currency = "USD",
                        Value = amount,
                    },
                }
            }
        };

        var requestPayload = JsonSerializer.Serialize(payoutRequest);
        var requestContent = new StringContent(requestPayload, Encoding.UTF8, "application/json");
        
        var response = await httpClient.PostAsync(PaypalPayoutsEndpoint, requestContent);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Status {response.StatusCode} Failed to create PayPal payout: {responseContent}");
        }
        
        var payoutResponse = JsonSerializer.Deserialize<PaypalPayoutResponse>(responseContent);

        if (payoutResponse == null)
        {
            throw new Exception($"Could not deserialize PayPal payout response to {typeof(PaypalPayoutResponse)}");
        }
        
        Logger.LogInfo($"Created PayPal payout with batch ID {payoutResponse.BatchHeader.PayoutBatchId}");
        return payoutResponse;
    }

    public async Task<PaypalRefundResponse> RefundPayment(string payPalCaptureId)
    {
        var accessToken = await GetAccessToken();

        var httpClient = _httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        httpClient.DefaultRequestHeaders.Add("Prefer", "return=representation");
        httpClient.DefaultRequestHeaders.Add("PayPal-Request-Id", Guid.NewGuid().ToString());

        var requestContent = new StringContent(string.Empty);
        requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        requestContent.Headers.ContentLength = 0;
        
        var url = PaypalRefundPaymentEndpoint.Replace("{}", payPalCaptureId);
        var response = await httpClient.PostAsync(url, requestContent);
        var responseContent = await response.Content.ReadAsStringAsync();
        
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Status {response.StatusCode} Failed to refund PayPal payment: {responseContent}");
        }

        var refundResponse = JsonSerializer.Deserialize<PaypalRefundResponse>(responseContent);

        if (refundResponse == null)
        {
            throw new Exception($"Could not deserialize PayPal refund response to {typeof(PaypalRefundResponse)}");
        }
        
        Logger.LogInfo($"Refunded PayPal payment with capture ID {payPalCaptureId}");
        return refundResponse;
    }

    private async Task<string> GetAccessToken()
    {
        if (!string.IsNullOrEmpty(_accessToken) && DateTime.UtcNow < _accessTokenExpiration)
        {
            return _accessToken;
        }
        
        var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_clientId}:{_clientSecret}"));
        
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
        
        var requestContent = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

        var response = await httpClient.PostAsync(PaypalAuthEndpoint, requestContent);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Status {response.StatusCode} Failed to fetch access token from PayPal: {responseContent}");
        }

        var tokenResponse = JsonSerializer.Deserialize<PaypalTokenResponse>(responseContent);
        
        if (tokenResponse == null)
        {
            throw new Exception($"Could not deserialize PayPal token response to {typeof(PaypalTokenResponse)}");
        }

        _accessToken = tokenResponse.AccessToken;
        _accessTokenExpiration = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn);
        
        return _accessToken;
    }
}