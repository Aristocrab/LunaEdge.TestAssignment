using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Extensions;

public static class ErrorOrExtensions
{
    public static ActionResult<T> ToActionResult<T>(this ErrorOr<T> result) where T : class
    {
        if (!result.IsError)
            return result.Value;

        return result.FirstError.ToProblemDetails();
    }
        
    public static ObjectResult ToProblemDetails(this Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError,
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = error.Type.ToString(),
            Detail = error.Description
        };

        return new ObjectResult(problemDetails) { StatusCode = statusCode };
    }
}