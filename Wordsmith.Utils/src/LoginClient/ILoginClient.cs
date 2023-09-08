using Wordsmith.Models;
using Wordsmith.Models.DataTransferObjects;
using Wordsmith.Models.RequestObjects;

namespace Wordsmith.Utils.LoginClient;

public interface ILoginClient
{
    /// <summary>
     /// Sends a request to the IdentityServer token endpoint with the Resource Owner Password grant type and returns
    /// access and refresh tokens of the user
    /// </summary>
    /// <param name="request">The object containing the username and password of the user</param>
    /// <param name="clientId">Id of the client where the login is taking place (See Config.cs in IdentityServer)</param>
    /// <param name="clientSecret">The secret used to issue tokens by the IdentityServer for the specified client</param>
    /// <param name="scopeList">Space-separated list of scopes the user should be authorized for</param>
    /// <returns>An object containing the access token, refresh token and the access token expiry time</returns>
    public Task<UserLoginDto> RequestAccess(UserLoginRequest request, string clientId, string clientSecret, string scopeList);
    
    /// <summary>
    /// Sends a request to the IdentityServer token endpoint with the Resource Owner Password grant type and returns
    /// an access token that is refreshed based on the provided refresh token. The refresh token is rotated on each call of this method
    /// </summary>
    /// <param name="refreshToken">The refresh token issued for the user</param>
    /// <param name="clientId">Id of the client where the login is taking place (See Config.cs in IdentityServer)</param>
    /// <param name="clientSecret">The secret used to issue tokens by the IdentityServer for the specified client</param>
    /// <returns>An object containing the access token, refresh token and the access token expiry time</returns>
    public Task<UserLoginDto> RequestAccess(string refreshToken, string clientId, string clientSecret);
}