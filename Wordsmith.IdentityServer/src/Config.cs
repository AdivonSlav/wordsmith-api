using Duende.IdentityServer.Models;

namespace Wordsmith.IdentityServer;

public static class Config
{
    private static string _userClientSecret;
    private static string _adminClientSecret;

    /// <summary>
    /// Initializes client secrets for IdentityServer
    /// </summary>
    /// <param name="userClientSecret">User client secret</param>
    /// <param name="adminClientSecret">Admin client secret</param>
    public static void InitSecrets(string userClientSecret, string adminClientSecret)
    {
        _userClientSecret = userClientSecret.Sha512();
        _adminClientSecret = adminClientSecret.Sha512();
    }
    
    public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>()
    {
        new("wordsmith_api.read", "API read access"),
        new("wordsmith_api.write", "API write access"),
        new("wordsmith_api.full_access", "API full access")
    };

    public static IEnumerable<ApiResource> ApiResources => new List<ApiResource>()
    {
        new("wordsmith_api")
        {
            Scopes = new List<string>() { "wordsmith_api.read", "wordsmith_api.write", "wordsmith_api.full_access" },
            ApiSecrets = new List<Secret>() { new(_userClientSecret), new(_adminClientSecret) },
            UserClaims = new List<string>() { "role", "user_ref_id" }
        }
    };

    public static IEnumerable<Client> Clients => new List<Client>()
    {
        new()
        {
            ClientId = "user.client",
            ClientName = "User Flutter client",
            AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
            ClientSecrets =
            {
                new Secret(_userClientSecret)
            },
            AllowedScopes = { "wordsmith_api.read", "wordsmith_api.write" },
            AllowOfflineAccess = true,
            AccessTokenLifetime = 60 * 15, // 15 minutes (in seconds)
            RefreshTokenUsage = TokenUsage.OneTimeOnly,
            RefreshTokenExpiration = TokenExpiration.Absolute,
            AbsoluteRefreshTokenLifetime = 60 * 30 * 1440 // 30 days (in seconds)
        },
        new()
        {
            ClientId = "admin.client",
            ClientName = "Admin Flutter client",
            AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
            ClientSecrets =
            {
                new Secret(_adminClientSecret)
            },
            AllowedScopes = { "wordsmith_api.full_access" },
            AllowOfflineAccess = true,
            AccessTokenLifetime = 60 * 15, // 15 minutes (in seconds)
            RefreshTokenUsage = TokenUsage.OneTimeOnly,
            RefreshTokenExpiration = TokenExpiration.Absolute,
            AbsoluteRefreshTokenLifetime = 60 * 30 * 1440 // 30 days (in seconds)
        }
    };

    public static IEnumerable<IdentityResource> IdentityResources => new List<IdentityResource>()
    {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile()
    };
}