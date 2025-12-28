using Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Modules.Workspaces.Infrastructure.ServiceInstallers;

/// <summary>
/// Represents the workspaces module endpoints service installer.
/// </summary>
internal sealed class EndpointsServiceInstaller : IServiceInstaller
{
    /// <inheritdoc />
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers().AddApplicationPart(Endpoints.AssemblyReference.Assembly);
    }
}
