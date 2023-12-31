using Microsoft.EntityFrameworkCore;
using Wordsmith.DataAccess.Db;
using Wordsmith.Utils;

namespace Wordsmith.API.Startup;

public static class DatabaseSetup
{
    public static IApplicationBuilder ConfigureDatabase(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = scope.ServiceProvider.GetService<DatabaseContext>();

        if (context == null) throw new Exception("DatabaseContext is not registered as a service!");

        Logger.LogInfo("Checking for any pending database migrations...");
        context.Database.Migrate();

        return app;
    }
}