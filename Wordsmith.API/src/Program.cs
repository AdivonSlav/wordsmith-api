using Microsoft.AspNetCore.Hosting.Server.Features;
using Wordsmith.API.Startup;
using Wordsmith.Utils;

#pragma warning disable IDE0058

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Configuration.AddEnvironmentVariables(prefix: "WORDSMITH_");
    
    UtilitySetup.Init(builder.Configuration, builder.Environment);
    
    builder.Services.RegisterStandardServices(builder.Configuration);
    builder.Services.RegisterAuthServices(builder.Configuration);
    builder.Services.RegisterDatabaseServices(builder.Configuration);

    var app = builder.Build();
    
    app.ConfigureDatabase();
    app.ConfigureSwagger();
    app.RegisterMiddleware();
    app.MapControllers();
    app.UseStaticFiles();

    Logger.LogInfo($"Up and running!");
    
    var addressEnv = Environment.GetEnvironmentVariable("ASPNETCORE_URLS");

    if (!string.IsNullOrEmpty(addressEnv))
    {
        var addresses = addressEnv.Split(";");

        foreach (var address in addresses)
        {
            Logger.LogInfo($"Listening on {address}");
        }
    }
    
    app.Run();
}
catch (Exception e)
{
    var type = e.GetType().Name;

    if (type.Equals("HostAbortedException", StringComparison.Ordinal))
    {
        throw;
    }
    
    Logger.LogFatal("API bootstrapping failed due to an exception", e);
    return 1;
}
finally
{
    Logger.Cleanup();
}

return 0;
