using Infrastructure.Configuration;

namespace App.ServiceInstallers.Swagger;

/// <summary>
/// Represents the swagger service installer.
/// </summary>
internal sealed class SwaggerServiceInstaller : IServiceInstaller
{
    /// <summary>
    /// Configures the provided service collection to enable and customize Swagger for the application.
    /// </summary>
    /// <param name="services">The IoC service collection to configure.</param>
    /// <param name="configuration">Application configuration (present for installer signature; not used directly by this method).</param>
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptions<SwaggerGenOptionsSetup>();

        services.ConfigureOptions<SwaggerUIOptionsSetup>();
        
        services.AddSwaggerGen();
    }
}