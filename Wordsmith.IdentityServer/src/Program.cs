using Wordsmith.IdentityServer.Startup;
using Wordsmith.Utils;
using Wordsmith.Utils.RabbitMQ;

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Configuration.AddEnvironmentVariables(prefix: "WORDSMITH_");
    
    Logger.Init(builder.Configuration["Logging:NLog:LogLevel"] ?? "Debug");
    RabbitService.Init(
        builder.Configuration["Connection:RabbitMQ:Host"],
        builder.Configuration["Connection:RabbitMQ:User"],
        builder.Configuration["Connection:RabbitMQ:Password"]);

    builder.Services.RegisterStandardServices();
    builder.Services.RegisterDatabaseServices(builder.Configuration);
    
    var app = builder.Build();

    app.ConfigureDatabase();
    app.RegisterMiddleware();
    app.EnsureSeedData(builder.Configuration);

    Logger.LogInfo("Up and running");
    
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
    
    Logger.LogFatal("IdentityServer bootstrapping failed due to an exception", e);
    return 1;
}
finally
{
    Logger.Cleanup();
}

return 0;
