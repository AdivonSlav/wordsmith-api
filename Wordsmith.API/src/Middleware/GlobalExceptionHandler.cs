using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Wordsmith.Models.Exceptions;
using Wordsmith.Utils;

namespace Wordsmith.API.Middleware;

public class GlobalExceptionHandler : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            string exceptionType;
            
            if (e is AppException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                exceptionType = "Bad request error";
                
                Logger.LogError("Bad request error", e);
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                exceptionType = "Internal server error";
                
                Logger.LogError("Internal server error", e);
            }
            
            context.Response.ContentType = "application/json";

            var problemDetails = new ProblemDetails()
            {
                Type = exceptionType,
                Detail = e.Message,
                Status = context.Response.StatusCode,
            };

            foreach (var key in e.Data.Keys)
            {
                problemDetails.Extensions.Add(key.ToString() ?? string.Empty, e.Data[key]);
            }

            var jsonProblemDetails = JsonSerializer.Serialize(problemDetails);
            await context.Response.WriteAsync(jsonProblemDetails);
        }
    }
}