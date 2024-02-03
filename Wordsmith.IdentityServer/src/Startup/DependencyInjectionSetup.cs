using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Wordsmith.IdentityServer.Db;
using Wordsmith.IdentityServer.Db.Entities;
using Wordsmith.IdentityServer.HostedServices;
using Wordsmith.IdentityServer.Middleware;
using Wordsmith.Utils.RabbitMQ;

namespace Wordsmith.IdentityServer.Startup;

public static class DependencyInjectionSetup
{
    public static IServiceCollection RegisterDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        var userClientSecret = configuration.GetSection("IdentityServer")["Secrets:User"];
        var adminClientSecret = configuration.GetSection("IdentityServer")["Secrets:Admin"];

        if (string.IsNullOrEmpty(userClientSecret))
        {
            throw new Exception("No user client secret was passed for IdentityServer configuration!");
        }
        if (string.IsNullOrEmpty(adminClientSecret))
        {
            throw new Exception("No admin client secret was passed for IdentityServer configuration!");
        }
        
        Config.InitSecrets(userClientSecret, adminClientSecret);
        
        var mysqlDetails = configuration.GetSection("Connection").GetSection("MySQL");
        var mysqlConnectionString = $"Server={mysqlDetails["Host"]};" +
                                    $"Port={mysqlDetails["Port"]};" +
                                    $"Uid={mysqlDetails["User"]};" +
                                    $"Pwd={mysqlDetails["Password"]};" +
                                    $"Database={mysqlDetails["Database"]};";
        var mysqlVersion = ServerVersion.AutoDetect(mysqlConnectionString);
        var migrationsAssembly = typeof(Config).Assembly.GetName().Name;
        
        services.AddDbContext<IdentityDatabaseContext>(options =>
        {
            options.UseMySql(mysqlConnectionString, mysqlVersion,
                sqlOptions => { sqlOptions.MigrationsAssembly(migrationsAssembly); });
        });
        
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 1;
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<IdentityDatabaseContext>();
        
        services.AddIdentityServer(options =>
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
        
        return services;
    }

    public static IServiceCollection RegisterStandardServices(this IServiceCollection services)
    {
        services.AddTransient<IdentityServerExceptionHandler>();
        services.AddSingleton<IMessageListener, MessageListener>();
        services.AddHostedService<UserPersistenceHostedService>();
        
        return services;
    }
}