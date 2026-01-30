namespace DndWebApp.Api.Middlewares.ExceptionHandling;

using System.Text.Json;

public class ExceptionHandler
{
    private readonly RequestDelegate next;
    private readonly ILogger<ExceptionHandler> logger;

    public ExceptionHandler(RequestDelegate next, ILogger<ExceptionHandler> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        return exception switch
        {
            CustomException e => WriteResponseAsync(context, e.StatusCode, e.Message),
            System.ComponentModel.DataAnnotations.ValidationException => WriteResponseAsync(context, 400, exception.Message),
            ArgumentException => WriteResponseAsync(context, 400, exception.Message),
            NotSupportedException => WriteResponseAsync(context, 405, exception.Message),
            InvalidOperationException => WriteResponseAsync(context, 409, exception.Message),
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