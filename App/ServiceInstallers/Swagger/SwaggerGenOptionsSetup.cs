using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace App.ServiceInstallers.Swagger;

/// <summary>
/// Represents the <see cref="SwaggerGenOptions"/> setup
/// </summary>
internal sealed class SwaggerGenOptionsSetup : IConfigureOptions<SwaggerGenOptions>
{
    /// <inheritdoc />
    public void Configure(SwaggerGenOptions options)
    {
        options.EnableAnnotations();
        
        options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "0.0.1",
                Title = "Modulith API",
                Description = "This swagger document describes the available API endpoints."
            }
        );
        
        options.TagActionsBy(selector =>
            {
                SwaggerOperationAttribute? operation = selector.ActionDescriptor
                    .EndpointMetadata
                    .OfType<SwaggerOperationAttribute>()
                    .FirstOrDefault();

                return operation?.Tags ?? ["Default"];
            }    
        );
        
        options.CustomSchemaIds(type => type.FullName);
    }
}
