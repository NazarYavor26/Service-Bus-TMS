using Newtonsoft.Json;
using Service_Bus_TMS.API.Exceptions;

namespace Service_Bus_TMS.API.Middlewares;

public class GlobalErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        string message = "An error occurred while processing your request.";

        if (exception is NotFoundException)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            message = exception.Message;
        }
        else if (exception is BadRequestException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            message = exception.Message;
        }

        var result = JsonConvert.SerializeObject(new { error = message });
        return context.Response.WriteAsync(result);
    }
}