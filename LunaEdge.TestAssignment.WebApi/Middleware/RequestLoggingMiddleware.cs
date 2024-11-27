using System.Diagnostics;
namespace LunaEdge.TestAssignment.WebApi.Middleware;

public sealed class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if(context.Request.Path.StartsWithSegments("/webhook"))
        {
            await _next(context);
            return;
        }
            
        _logger.LogInformation("{RequestMethod} {RequestPath} started", 
            context.Request.Method, 
            context.Request.Path);
            
        var stopwatch = new Stopwatch();
            
        stopwatch.Start();
        await _next(context);
        stopwatch.Stop();
            
        _logger.LogInformation("{RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.000} ms",
            context.Request.Method, 
            context.Request.Path, 
            context.Response.StatusCode, 
            stopwatch.Elapsed.TotalMilliseconds);
    }
}