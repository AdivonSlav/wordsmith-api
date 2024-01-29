using Duende.IdentityServer.Models;

namespace Wordsmith.IdentityServer;

public static class Config
{
    public static string? UserClientSecret { get; set; }
    public static string? AdminClientSecret { get; set; }

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
            ApiSecrets = new List<Secret>() { new(UserClientSecret.Sha256()), new(AdminClientSecret.Sha256()) },
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
                new Secret(UserClientSecret.Sha256())
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
                new Secret(AdminClientSecret.Sha256())
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