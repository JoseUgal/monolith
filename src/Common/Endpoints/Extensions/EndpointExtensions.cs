using Domain.Errors;
using Domain.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Endpoints.Extensions;

/// <summary>
/// Contains extension methods for the <see cref="Endpoint"/> class.
/// </summary>
public static class EndpointExtensions
{
    /// <summary>
    /// Handles the failure result and returns the appropriate response.
    /// </summary>
    /// <param name="endpoint">The endpoint.</param>
    /// <param name="result">The failure result.</param>
    /// <returns>The appropriate response based on the result type.</returns>
    /// <exception cref="InvalidOperationException"> when this method is invoked with a success result.</exception>
    public static ActionResult HandleFailure(this Endpoint endpoint, Result result)
    {
        if (result.IsSuccess)
        {
            throw new InvalidOperationException(
                "This method can't be invoked for a success result."
            );
        }

        if (result.Error is ValidationError validationError)
        {
            return endpoint.BadRequest(
                CreateProblemDetails(
                    "Validation Error",
                    StatusCodes.Status400BadRequest,
                    validationError,
                    validationError.Errors
                )
            );
        }

        return result.Error.Type switch
        {
            ErrorType.NotFound => endpoint.NotFound(
                CreateProblemDetails("Not Found", StatusCodes.Status404NotFound, result.Error)
            ),
            ErrorType.Conflict => endpoint.Conflict(
                CreateProblemDetails("Conflict", StatusCodes.Status409Conflict, result.Error)
            ),
            ErrorType.Forbidden => endpoint.StatusCode(
                StatusCodes.Status403Forbidden,
                CreateProblemDetails("Forbidden", StatusCodes.Status403Forbidden, result.Error)
            ),
            _ => endpoint.BadRequest(
                CreateProblemDetails("Bad Request", StatusCodes.Status400BadRequest, result.Error)
            )
        };
    }

    private static ProblemDetails CreateProblemDetails(string title, int status, Error error, Error[]? errors = null)
    {
        var problemDetails = new ProblemDetails
        {
            Title = title,
            Status = status,
            Type = error.Code,
            Detail = error.Description
        };

        if (errors is not null)
        {
            problemDetails.Extensions[nameof(errors)] = errors;
        }
        
        return problemDetails;
    }
}
