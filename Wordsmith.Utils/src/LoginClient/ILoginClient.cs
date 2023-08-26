using Wordsmith.Models;
using Wordsmith.Models.RequestObjects;

namespace Wordsmith.Utils.LoginClient;

public interface ILoginClient
{
    public Task<UserLoginDto> RequestAccess(UserLoginRequest request, string clientId, string clientSecret, string scopeList);
    public Task<UserLoginDto> RequestAccess(string refreshToken, string clientId, string clientSecret);
}