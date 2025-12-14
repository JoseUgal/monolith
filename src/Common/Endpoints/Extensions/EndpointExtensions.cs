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
    /// <summary>
    /// Converts a failed <paramref name="result"/> into an appropriate HTTP <see cref="ActionResult"/> carrying a ProblemDetails payload.
    /// </summary>
    /// <param name="result">The failed Result to translate into an HTTP response.</param>
    /// <returns>An ActionResult whose ProblemDetails describes the error: validation errors produce 400 Bad Request (with detailed errors), NotFound produces 404 Not Found, Conflict produces 409 Conflict, and other errors produce 400 Bad Request.</returns>
    /// <exception cref="InvalidOperationException">Thrown when this method is invoked with a success result.</exception>
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
            ErrorType.Conflict => endpoint.NotFound(
                CreateProblemDetails("Conflict", StatusCodes.Status409Conflict, result.Error)
            ),
            _ => endpoint.BadRequest(
                CreateProblemDetails("Bad Request", StatusCodes.Status400BadRequest, result.Error)
            )
        };
    }

    /// <summary>
    /// Builds a ProblemDetails object describing an error for an HTTP response.
    /// </summary>
    /// <param name="title">Human-readable title for the problem.</param>
    /// <param name="status">HTTP status code to set on the ProblemDetails.</param>
    /// <param name="error">Primary error; its Code is set as the ProblemDetails `Type` and its Description as the `Detail`.</param>
    /// <param name="errors">Optional additional error objects to include in ProblemDetails.Extensions under the key "errors".</param>
    /// <returns>A ProblemDetails instance populated with the provided title, status, type, detail, and optional errors extension.</returns>
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