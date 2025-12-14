using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Persistence.Options;

/// <summary>
/// Configures <see cref="ConnectionStringOptions"/> by retrieving the connection string
/// named "Database" from the provided <see cref="IConfiguration"/> instance.
/// </summary>
/// <remarks>
/// Throws <see cref="InvalidOperationException"/> if the connection string is missing or empty.
/// </remarks>
internal sealed class ConnectionStringSetup(IConfiguration configuration) : IConfigureOptions<ConnectionStringOptions>
{
    private const string ConnectionStringName = "Database";

    /// <summary>
    /// Sets the ConnectionStringOptions.Value to the "Database" connection string from the application's configuration.
    /// </summary>
    /// <param name="options">The options instance to populate with the resolved connection string.</param>
    /// <exception cref="InvalidOperationException">Thrown when the "Database" connection string is missing or consists only of whitespace.</exception>
    public void Configure(ConnectionStringOptions options)
    {
        string? connectionString = configuration.GetConnectionString(ConnectionStringName);

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException($"The connection string '{ConnectionStringName}' was not configured.");
        }
        
        options.Value = connectionString;
    }
}
