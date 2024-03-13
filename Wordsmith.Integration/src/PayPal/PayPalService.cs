using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Wordsmith.Integration.PayPal.Models;
using Wordsmith.Utils;

namespace Wordsmith.Integration.PayPal;

public class PayPalService : IPayPalService
{
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly IHttpClientFactory _httpClientFactory;

    private const string PayPalAuthEndpoint = "https://api-m.sandbox.paypal.com/v1/oauth2/token";
    private const string PayPalOrdersEndpoint = "https://api-m.sandbox.paypal.com/v2/checkout/orders";

    private string _accessToken;
    private DateTime _accessTokenExpiration;

    public PayPalService(string clientId, string clientSecret, IHttpClientFactory httpClientFactory)
    {
        _clientId = clientId;
        _clientSecret = clientSecret;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<PayPalOrderResponse> CreateOrder(decimal amount, string name, string description)
    {
        var accessToken = await GetAccessToken();
        
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        httpClient.DefaultRequestHeaders.Add("Prefer", "return=representation");
        httpClient.DefaultRequestHeaders.Add("PayPal-Request-Id", Guid.NewGuid().ToString());

        var orderRequest = new PayPalOrderRequest()
        {
            Intent = "CAPTURE",
            PurchaseUnits = new List<PaypalPurchaseUnit>()
            {
                new()
                {
                    ReferenceId = Guid.NewGuid().ToString(),
                    Amount = new PayPalAmount()
                    {
                      CurrencyCode  = "USD",
                      Value = amount.ToString(CultureInfo.InvariantCulture),
                      Breakdown = new PayPalBreakdown()
                      {
                          ItemTotal = new PayPalItemTotal()
                          {
                              CurrencyCode = "USD",
                              Value = amount.ToString(CultureInfo.InvariantCulture),
                          }
                      },
                    },
                    Items = new List<PayPalItem>()
                    {
                        new()
                        {
                            Name = name,
                            Description = description,
                            Quantity = "1",
                            UnitAmount = new PayPalUnitAmount()
                            {
                                CurrencyCode = "USD",
                                Value = amount.ToString(CultureInfo.InvariantCulture),
                            }
                        }
                    },
                }
            },
            PaymentSource = new PayPalPaymentSource()
            {
                Paypal = new PayPalPayment()
                {
                    ExperienceContext = new PayPalExperienceContext()
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

        var response = await httpClient.PostAsync(PayPalOrdersEndpoint, requestContent);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to create PayPal order with code {response.StatusCode}: {responseContent}");
        }
        
        var orderResponse = JsonSerializer.Deserialize<PayPalOrderResponse>(responseContent);

        if (orderResponse == null)
        {
            throw new Exception("Could not deserialize");
        }
        
        Logger.LogInfo($"Created PayPal order with reference ID {orderResponse.PurchaseUnits.First().ReferenceId}");
        return orderResponse;
    }

    private async Task<string> GetAccessToken()
    {
        if (!string.IsNullOrEmpty(_accessToken) && DateTime.UtcNow < _accessTokenExpiration)
        {
            return _accessToken;
        }
        
        var httpClient = _httpClientFactory.CreateClient();
        
        var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_clientId}:{_clientSecret}"));
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
        var requestContent = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

        var response = await httpClient.PostAsync(PayPalAuthEndpoint, requestContent);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to fetch access token from PayPal: {responseContent}");
        }

        var tokenResponse = JsonSerializer.Deserialize<PayPalTokenResponse>(responseContent);
        
        if (tokenResponse == null)
        {
            throw new Exception("Could not deserialize PayPal token response!");
        }

        _accessToken = tokenResponse.AccessToken;
        _accessTokenExpiration = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn);
        
        return _accessToken;
    }
}