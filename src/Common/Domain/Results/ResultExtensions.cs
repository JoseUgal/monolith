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
    /// <summary>
    /// Attempts to extract the contained value from a Result&lt;T&gt;, producing an Error when the result represents failure.
    /// </summary>
    /// <typeparam name="T">The type of the contained value.</typeparam>
    /// <param name="result">The Result to inspect.</param>
    /// <param name="value">Receives the contained value when successful, otherwise receives the default value for <typeparamref name="T"/>.</param>
    /// <param name="error">Receives the associated Error when the result is a failure, otherwise receives <see cref="Error.None"/>.</param>
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