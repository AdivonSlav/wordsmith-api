using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Wordsmith.IdentityServer;
using Wordsmith.IdentityServer.Db;
using Wordsmith.IdentityServer.Db.Entities;
using Wordsmith.IdentityServer.HostedServices;
using Wordsmith.IdentityServer.Middleware;
using Wordsmith.Utils;
using Wordsmith.Utils.RabbitMQ;

try
{
    var builder = WebApplication.CreateBuilder(args);
    Logger.Init(builder.Configuration["Logging:NLog:LogLevel"] ?? "Debug");
    RabbitService.Init(
        builder.Configuration["Connection:RabbitMQ:Host"],
        builder.Configuration["Connection:RabbitMQ:User"],
        builder.Configuration["Connection:RabbitMQ:Password"]);

    Config.UserClientSecret = builder.Configuration.GetSection("IdentityServer")["Secrets:User"];
    Config.AdminClientSecret = builder.Configuration.GetSection("IdentityServer")["Secrets:Admin"];
    
    var mysqlDetails = builder.Configuration.GetSection("Connection").GetSection("MySQL");
    var mysqlConnectionString = $"Server={mysqlDetails["Host"]};" +
                                $"Port={mysqlDetails["Port"]};" +
                                $"Uid={mysqlDetails["User"]};" +
                                $"Pwd={mysqlDetails["Password"]};" +
                                $"Database={mysqlDetails["Database"]};";
    var mysqlVersion = ServerVersion.AutoDetect(mysqlConnectionString);
    var migrationsAssembly = typeof(Config).Assembly.GetName().Name;
    
    builder.Services.AddTransient<IdentityServerExceptionHandler>();
    builder.Services.AddDbContext<IdentityDatabaseContext>(options =>
    {
        options.UseMySql(mysqlConnectionString, mysqlVersion,
            sqlOptions => { sqlOptions.MigrationsAssembly(migrationsAssembly); });
    });
    builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<IdentityDatabaseContext>();
    builder.Services.AddIdentityServer(options =>
        {
            options.Events.RaiseErrorEvents = true;
            options.Events.RaiseInformationEvents = true;
            options.Events.RaiseFailureEvents = true;
            options.Events.RaiseSuccessEvents = true;

            options.EmitStaticAudienceClaim = true;
        })
        .AddConfigurationStore(options => options.ConfigureDbContext = b => b.UseMySql(mysqlConnectionString, mysqlVersion,
            sqlOptions => { sqlOptions.MigrationsAssembly(migrationsAssembly); }))
        .AddOperationalStore(options => options.ConfigureDbContext = b => b.UseMySql(mysqlConnectionString, mysqlVersion,
            sqlOptions => { sqlOptions.MigrationsAssembly(migrationsAssembly); }))
        .AddAspNetIdentity<ApplicationUser>();
    builder.Services.AddSingleton<IMessageListener, MessageListener>();
    builder.Services.AddHostedService<UserPersistenceHostedService>();

    var app = builder.Build();

    app.UseMiddleware<IdentityServerExceptionHandler>();
    app.UseIdentityServer();

    if (args.Contains("--seed"))
    {
        Logger.LogInfo("Seeding database...");
        SeedData.EnsureSeedData(app);
        return 0;
    }

    Logger.LogInfo("Listening...");
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
