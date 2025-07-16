using Microsoft.AspNetCore.Http.Extensions;

namespace W2B.S3.Middlewares;

public sealed class NotEmptyFilesMiddleware(RequestDelegate next, ILogger<NotEmptyFilesMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var url = context.Request.GetEncodedUrl();
        var method = context.Request.Method.ToLower();

        if (url.Contains("/object") && method == "post")
        {
            logger.LogInformation("worked");
        }

        await next(context);
    }
}

public static class NotEmptyFilesMiddlewareExtensions
{
    public static IApplicationBuilder UseNonEmptyFiles(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<NotEmptyFilesMiddleware>();
    }
}