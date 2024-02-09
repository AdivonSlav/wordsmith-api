using IdentityModel.Client;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.Exceptions;
using Wordsmith.Models.RequestObjects;
using Wordsmith.Models.RequestObjects.User;

namespace Wordsmith.Utils.LoginClient;

public class LoginClient : ILoginClient
{
    private readonly string _identityAddress;
    private readonly IHttpClientFactory _clientFactory;

    /// <summary>
    /// Instantiates a client for managing login requests to the Identity Server
    /// </summary>
    /// <param name="address">Base address of the Identity Server</param>
    /// <param name="clientFactory">A factory that manages the lifetime and creation of new http clients</param>
    public LoginClient(string address, IHttpClientFactory clientFactory)
    {
        _identityAddress = address;
        _clientFactory = clientFactory;
    }
    
    public async Task<UserLoginDto> RequestAccess(UserLoginRequest request, string clientId, string clientSecret, string scopeList)
    {
        var client = _clientFactory.CreateClient();
        var discoveryDoc = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest()
        {
            Address = _identityAddress,
            Policy = { ValidateIssuerName = false, ValidateEndpoints = false }
        });

        if (discoveryDoc.IsError)
        {
            throw new Exception(
                $"Unable to retrieve discovery document from the Identity Server at {_identityAddress}",
                innerException: new Exception(discoveryDoc.Error));
        }

        var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest()
        {
            Address = discoveryDoc.TokenEndpoint,
            UserName = request.Username,
            Password = request.Password,
            ClientId = clientId,
            ClientSecret = clientSecret,
            Scope = scopeList,
        });

        if (tokenResponse.IsError)
        {
            throw new AppException("Unable to login", new Dictionary<string, object>()
            {
                { "reason", tokenResponse.ErrorDescription ?? string.Empty }
            });
        }

        return new UserLoginDto()
        {
            AccessToken = tokenResponse.AccessToken,
            RefreshToken = tokenResponse.RefreshToken,
            ExpiresIn = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn)
        };
    }

    public async Task<UserLoginDto> RequestAccess(string refreshToken, string clientId, string clientSecret)
    {
        var client = _clientFactory.CreateClient();
        var discoveryDoc = await client.GetDiscoveryDocumentAsync(_identityAddress);

        if (discoveryDoc.IsError)
        {
            throw new Exception(
                $"Unable to retrieve discovery document from the Identity Server at {_identityAddress}",
                innerException: new Exception(discoveryDoc.Error));
        }

        var tokenResponse = await client.RequestRefreshTokenAsync(new RefreshTokenRequest()
        {
            Address = discoveryDoc.TokenEndpoint,
            RefreshToken = refreshToken,
            ClientId = clientId,
            ClientSecret = clientSecret,
        });
        
        if (tokenResponse.IsError)
        {
            throw new AppException("Unable to refresh login", new Dictionary<string, object>()
            {
                { "reason", tokenResponse.ErrorDescription ?? string.Empty }
            });
        }
        
        return new UserLoginDto()
        {
            AccessToken = tokenResponse.AccessToken,
            RefreshToken = tokenResponse.RefreshToken,
            ExpiresIn = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn)
        };
    }

    public async Task<UserLoginDto> VerifyAccess(string accessToken, string clientId, string clientSecret)
    {
        var client = _clientFactory.CreateClient();
        var discoveryDoc = await client.GetDiscoveryDocumentAsync(_identityAddress);
        
        if (discoveryDoc.IsError)
        {
            throw new Exception(
                $"Unable to retrieve discovery document from the Identity Server at {_identityAddress}",
                innerException: new Exception(discoveryDoc.Error));
        }

        var response = await client.IntrospectTokenAsync(new TokenIntrospectionRequest()
        {
            Address = discoveryDoc.IntrospectionEndpoint,
            Token = accessToken,
            ClientId = "wordsmith_api",
            ClientSecret = clientSecret
        });

        if (response.IsError)
        {
            throw new AppException("Unable verify access token validity", new Dictionary<string, object>()
            {
                { "reason", response.Error ?? string.Empty }
            });
        }

        return new UserLoginDto()
        {
            AccessToken = response.IsActive ? accessToken : null,
            RefreshToken = null,
            ExpiresIn = null,
        };
    }
}

