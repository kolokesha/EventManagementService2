using System.ComponentModel.DataAnnotations;

namespace EventManagementService.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext httpContext, ILoggerFactory loggerFactory)
    {
        var logger = loggerFactory.CreateLogger(nameof(ExceptionHandlingMiddleware));
        try
        {
            await next(httpContext);
        }
        catch (ValidationException ex)
        {
            logger.LogWarning(ex, "Validation error");

            httpContext.Response.StatusCode = 400;

            var response = new
            {
                Message = ex.Message
            };

            await httpContext.Response.WriteAsJsonAsync(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled error");

            httpContext.Response.StatusCode = 500;

            await httpContext.Response.WriteAsJsonAsync(new
            {
                Message = "Внутренняя ошибка сервера"
            });
        }
    }
}