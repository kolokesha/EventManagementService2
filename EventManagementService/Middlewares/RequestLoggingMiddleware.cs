namespace EventManagementService.Middlewares;

public class RequestLoggingMiddleware(RequestDelegate next)
{
    public async  Task InvokeAsync(HttpContext httpContext)
    {
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Request: {httpContext.Request.Method} {httpContext.Request.Path}");
        httpContext.Response.OnStarting(() =>
        {
            httpContext.Response.Headers.Append("X-Custom-Header", "MyApp");
            return Task.CompletedTask;
        });
        
        await next(httpContext);
        
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Response: {httpContext.Response.StatusCode}");
    }
}