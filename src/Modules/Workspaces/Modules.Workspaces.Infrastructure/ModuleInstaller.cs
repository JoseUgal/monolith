using Infrastructure.Configuration;
using Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Modules.Workspaces.Infrastructure;

/// <summary>
/// Represents the workspaces module installer.
/// </summary>
public sealed class ModuleInstaller : IModuleInstaller
{
    /// <inheritdoc />
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.InstallServicesFromAssemblies(configuration, AssemblyReference.Assembly);

        services.AddScopedAsMatchingInterfaces(Workspaces.Persistence.AssemblyReference.Assembly);
    }
}
