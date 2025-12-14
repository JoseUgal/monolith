using Domain.Errors;

namespace Domain.Results;

/// <summary>
/// Contains extension methods for working with the <see cref="Result"/> class.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Attempts to extract the value from a <see cref="Result{T}"/>.  
    /// Returns <c>true</c> if the result is successful; otherwise returns <c>false</c>  
    /// and outputs the associated <see cref="Error"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value contained in the result.</typeparam>
    /// <param name="result">The result to inspect.</param>
    /// <param name="value">The extracted value when the operation succeeds.</param>
    /// <param name="error">The error when the operation fails.</param>
    /// <returns><c>true</c> if the result is successful; otherwise <c>false</c>.</returns>
    public static bool TryGetValue<T>(this Result<T> result, out T value, out Error error)
    {
        if (result.IsFailure)
        {
            value = default!;
            error = result.Error;
            
            return false;
        }
        
        value = result.Value;
        error = Error.None;
        
        return true;
    }
}
