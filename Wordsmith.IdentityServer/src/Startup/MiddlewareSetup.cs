using Wordsmith.IdentityServer.Middleware;

namespace Wordsmith.IdentityServer.Startup;

public static class MiddlewareSetup
{
    public static IApplicationBuilder RegisterMiddleware(this IApplicationBuilder app)
    {
        app.UseHttpsRedirection();
        app.UseMiddleware<IdentityServerExceptionHandler>();
        app.UseIdentityServer();
        
        return app;
    }
}