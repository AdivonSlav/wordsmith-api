using Duende.IdentityServer.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using Wordsmith.IdentityServer.Db;
using Wordsmith.Utils;

namespace Wordsmith.IdentityServer.Startup;

public static class DatabaseSetup
{
    public static IApplicationBuilder ConfigureDatabase(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var configurationContext = scope.ServiceProvider.GetService<ConfigurationDbContext>();
        var persistedGrantsContext = scope.ServiceProvider.GetService<PersistedGrantDbContext>();
        var identityContext = scope.ServiceProvider.GetService<IdentityDatabaseContext>();

        if (configurationContext == null) throw new Exception("ConfigurationDbContext is not registered as a service!");
        if (persistedGrantsContext == null) throw new Exception("PersistedGrantDbContext is not registered as a service!");
        if (identityContext == null) throw new Exception("IdentityDatabaseContext is not registered as a service!");

        Logger.LogInfo("Checking for any pending database migrations...");
        configurationContext.Database.Migrate();
        persistedGrantsContext.Database.Migrate();
        identityContext.Database.Migrate();

        return app;
    }
}