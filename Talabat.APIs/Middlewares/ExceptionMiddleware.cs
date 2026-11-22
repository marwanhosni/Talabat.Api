using Talabat.APIs.Errors;
using System.Text.Json;

namespace Talabat.APIs.Middlewares;

public class ExceptionMiddleware
{
    private readonly IHostEnvironment _env;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(IHostEnvironment env , ILogger<ExceptionMiddleware> logger , RequestDelegate next)
    {
        _env = env;
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try 
        {
            await _next.Invoke(context);
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, ex.Message);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500;

            var response = _env.IsDevelopment() ? new ApiExceptionResponse(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                : new ApiExceptionResponse(context.Response.StatusCode, "Internal Server Error");
            //var options = new JsonSexrializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            //var json = JsonSerializer.Serialize(response, options);
            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}
