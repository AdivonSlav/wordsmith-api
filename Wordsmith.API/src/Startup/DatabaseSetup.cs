using Microsoft.EntityFrameworkCore;
using Wordsmith.DataAccess.Db;
using Wordsmith.DataAccess.Db.Seeds;
using Wordsmith.Utils;

namespace Wordsmith.API.Startup;

public static class DatabaseSetup
{
    public static async Task<IApplicationBuilder> ConfigureDatabase(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = scope.ServiceProvider.GetService<DatabaseContext>();

        if (context == null) throw new Exception("DatabaseContext is not registered as a service!");

        Logger.LogInfo("Checking for any pending database migrations...");
        await context.Database.MigrateAsync();
 
        await DatabaseSeeds.EnsureSeedData(context);
        
        return app;
    }
}