using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Wordsmith.CommunicationsRelay;
using Wordsmith.CommunicationsRelay.HostedServices;
using Wordsmith.CommunicationsRelay.Services;
using Wordsmith.Utils;
using Wordsmith.Utils.RabbitMQ;

try
{
    var builder = Host.CreateDefaultBuilder()
        .ConfigureAppConfiguration(configuration =>
        {
            configuration.AddJsonFile("appsettings.json", optional: false);
            configuration.AddJsonFile("appsettings.Development.json", optional: true);
            configuration.AddEnvironmentVariables(prefix: "WORDSMITH_");
        })
        .ConfigureServices((hostContext, services) =>
        {
            var config = hostContext.Configuration;
            var rabbitConnection = config.GetSection("Connection:RabbitMQ");
            
            Logger.Init(config["Logging:NLog:LogLevel"] ?? "Debug");
            RabbitService.Init(rabbitConnection["Host"], rabbitConnection["User"], rabbitConnection["Password"]);

            services.Configure<EmailSettings>(config.GetSection("EmailSettings"));
            services.AddTransient<IMessageListener, MessageListener>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddHostedService<EmailBackgroundService>();
        })
        .UseConsoleLifetime()
        .Build();

    Logger.LogInfo("Starting up...");
    builder.Run();
}
catch (Exception e)
{
    Logger.LogFatal("Encountered fatal error", e);
    return 1;
}
finally
{
    Logger.Cleanup();
}

return 0;
