using Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Modules.Workspaces.Infrastructure.ServiceInstallers;

/// <summary>
/// Represents the workspaces module application service installer.
/// </summary>
internal class ApplicationServiceInstaller : IServiceInstaller
{
    /// <inheritdoc />
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(Application.AssemblyReference.Assembly);
            }
        );
    }
}
