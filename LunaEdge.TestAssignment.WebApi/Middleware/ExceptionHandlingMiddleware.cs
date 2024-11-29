using FluentValidation;
using LunaEdge.TestAssignment.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace LunaEdge.TestAssignment.WebApi.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IProblemDetailsService _problemDetailsService;

    public ExceptionHandlingMiddleware(RequestDelegate next, IProblemDetailsService problemDetailsService)
    {
        _next = next;
        _problemDetailsService = problemDetailsService;
    }

    public async Task InvokeAsync(HttpContext context)
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

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = exception switch
        {
            ArgumentException => StatusCodes.Status400BadRequest,
            InvalidPasswordException => StatusCodes.Status401Unauthorized,
            UserNotFoundException or TaskNotFoundException => StatusCodes.Status404NotFound,
            UserAlreadyExistsException => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };
        
        var errorMessage = exception switch
        {
            ValidationException validationException => validationException.Errors.First().ErrorMessage,
            _ => exception.Message
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = "An error occurred",
            Type = exception.GetType().Name,
            Detail = errorMessage
        };

        // Use IProblemDetailsService to write the response
        context.Response.StatusCode = statusCode;
        await _problemDetailsService.WriteAsync(new ProblemDetailsContext
        {
            HttpContext = context,
            ProblemDetails = problemDetails
        });
    }
}
