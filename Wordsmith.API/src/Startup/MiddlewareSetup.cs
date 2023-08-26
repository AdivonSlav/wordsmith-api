using Wordsmith.API.Middleware;

namespace Wordsmith.API.Startup;

public static class MiddlewareSetup
{
    public static IApplicationBuilder RegisterMiddleware(this IApplicationBuilder app)
    {
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<GlobalExceptionHandler>();
        
        return app;
    }
}