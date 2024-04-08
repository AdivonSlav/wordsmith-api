using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Wordsmith.CommunicationsRelay;
using Wordsmith.CommunicationsRelay.HostedServices;
using Wordsmith.CommunicationsRelay.Services;
using Wordsmith.Utils.RabbitMQ;

var builder = Host.CreateDefaultBuilder();

builder.ConfigureAppConfiguration(configuration =>
{
    configuration.AddJsonFile("appsettings.json", optional: false);
    configuration.AddJsonFile("appsettings.Development.json", optional: true);
});

builder.ConfigureServices((hostContext, services) =>
{
    var config = hostContext.Configuration;

    var rabbitConnection = config.GetSection("Connection:RabbitMQ");
    RabbitService.Init(rabbitConnection["Host"], rabbitConnection["User"], rabbitConnection["Password"]);
    
    services.Configure<EmailSettings>(config.GetSection("EmailSettings"));
    services.AddTransient<IMessageListener, MessageListener>();
    services.AddTransient<IEmailService, EmailService>();
    services.AddHostedService<EmailBackgroundService>();
});

await builder.RunConsoleAsync();
