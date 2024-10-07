using System.Net;
using System.Text.Json;
using API.Errors;

namespace API.Middleware;

public class ExceptionMiddleware(IHostEnvironment env, RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // If no error then continue to the next stage/middleware
            await next(context);
        }
        catch (Exception ex)
        {
            // If any exception, call our own exception handler
            await HandleExceptionAsync(context, ex, env);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex, IHostEnvironment env)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // Error 500

        // If in dev, also send the stacktrace otherwise only send the code and message
        var response = env.IsDevelopment()
            ? new ApiErrorResponse(context.Response.StatusCode, ex.Message, ex.StackTrace)
            : new ApiErrorResponse(context.Response.StatusCode, ex.Message, "Internal server error");
        
        var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};

        var json = JsonSerializer.Serialize(response, options);

        return context.Response.WriteAsync(json);
    }
}
