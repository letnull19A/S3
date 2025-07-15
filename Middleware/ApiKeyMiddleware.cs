using W2B.S3.Interfaces;
using W2B.S3.Services;

namespace W2B.S3.Middleware;

public class ApiKeyMiddleware(RequestDelegate next)
{
    private const string HeaderName = "X-API-Key";

    public async Task InvokeAsync(HttpContext context, IApiKeyService keyService)
    {
        if (!context.Request.Headers.TryGetValue(HeaderName, out var apiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API key required");
            return;
        }

        if (!await keyService.IsValidKeyAsync(apiKey!))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Invalid API key");
            return;
        }

        await next(context);
    }
}