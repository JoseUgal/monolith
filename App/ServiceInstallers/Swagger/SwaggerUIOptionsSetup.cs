using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace App.ServiceInstallers.Swagger;

/// <summary>
/// Represents the <see cref="SwaggerUIOptions"/> setup
/// </summary>
internal sealed class SwaggerUIOptionsSetup : IConfigureOptions<SwaggerUIOptions>
{
    /// <summary>
    /// Enables display of request durations in the Swagger UI.
    /// </summary>
    /// <param name="options">The Swagger UI options to configure.</param>
    public void Configure(SwaggerUIOptions options)
    {
        options.DisplayRequestDuration();
    }
}