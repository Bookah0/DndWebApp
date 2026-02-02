namespace DndWebApp.Api.Middlewares.ExceptionHandling;

using System.Text.Json;

public class ExceptionHandler(RequestDelegate next, ILogger<ExceptionHandler> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            LogError(ex);
            await HandleExceptionAsync(context, ex);
        }
    }

    private void LogError(Exception ex)
    {
        switch (ex)
        {
            case NotFoundException:
                logger.LogError(ex, "Not found: {Message}", ex.Message);
                break;
            case ValidationException:   
            case System.ComponentModel.DataAnnotations.ValidationException:
            case ArgumentException:
                logger.LogError(ex, "Validation error: {Message}", ex.Message);
                break;
            case ConflictException:
                logger.LogError(ex, "Conflict error: {Message}", ex.Message);
                break;
            case UnauthorizedException:
                logger.LogError(ex, "Unauthorized error: {Message}", ex.Message);
                break;
            default:
                logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
                break;
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        return ex switch
        {
            CustomException e => WriteResponseAsync(context, e.StatusCode, e.Message),
            System.ComponentModel.DataAnnotations.ValidationException => WriteResponseAsync(context, 400, ex.Message),
            ArgumentException => WriteResponseAsync(context, 400, ex.Message),
            NotSupportedException => WriteResponseAsync(context, 405, ex.Message),
            InvalidOperationException => WriteResponseAsync(context, 409, ex.Message),
            _ => WriteResponseAsync(context, 500, "An unexpected error occurred."),
        };
    }

    private static Task WriteResponseAsync(HttpContext context, int statusCode, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        var result = JsonSerializer.Serialize(new { error = message });
        return context.Response.WriteAsync(result);
    }
}