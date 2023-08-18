using IdentityModel.Client;

var client = new HttpClient();
var disco = await client.GetDiscoveryDocumentAsync("https://localhost:7443");

if (disco.IsError)
{
    Console.WriteLine(disco.Error);
    Console.WriteLine(disco.Exception?.InnerException);
    return;
}

var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest()
{
    Address = disco.TokenEndpoint,
    ClientId = "user.client",
    ClientSecret = "AUserClientSecretThatIsSupposedToBeSecure",
    UserName = "jane",
    Password = "Pass123$",
    Scope = "wordsmith_api.read offline_access"
});

if (tokenResponse.IsError)
{
    Console.WriteLine(tokenResponse.Error);
    Console.WriteLine(tokenResponse.Exception?.InnerException);
    return;
}

Console.WriteLine(tokenResponse.AccessToken);
Console.WriteLine("-----------------------------------------");
Console.WriteLine(tokenResponse.RefreshToken);

Console.WriteLine("Attempting to refresh access token...");

var refreshedTokenResponse = await client.RequestRefreshTokenAsync(new RefreshTokenRequest()
{
    Address = disco.TokenEndpoint,
    ClientId = "user.client",
    ClientSecret = "AUserClientSecretThatIsSupposedToBeSecure",
    RefreshToken = "50BAF8D43A302DC4931D2D327124003894ADBA281ADAA737E0E1F5529DFCEE73-1"
});

Console.WriteLine(refreshedTokenResponse.AccessToken);