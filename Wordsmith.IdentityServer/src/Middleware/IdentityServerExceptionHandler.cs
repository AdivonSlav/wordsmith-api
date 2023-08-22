
using Wordsmith.Utils;

namespace Wordsmith.IdentityServer.Middleware;

public class IdentityServerExceptionHandler : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            Logger.LogError("Internal server error", e);
        }
    }
}