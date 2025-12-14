using Infrastructure.Configuration;
using Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Modules.Users.Infrastructure;

/// <summary>
/// Represents the users module installer.
/// </summary>
public sealed class ModuleInstaller : IModuleInstaller
{
    /// <summary>
    /// Registers the Users module's services into the dependency injection container.
    /// </summary>
    /// <remarks>
    /// Scans module and persistence assemblies and registers discovered implementations with appropriate lifetimes (singletons and scoped services).
    /// </remarks>
    /// <param name="services">The service collection to add registrations to.</param>
    /// <param name="configuration">Configuration used when installing services from assemblies.</param>
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.InstallServicesFromAssemblies(configuration, AssemblyReference.Assembly);

        services.AddSingletonAsMatchingInterfaces(AssemblyReference.Assembly);
        
        services.AddScopedAsMatchingInterfaces(AssemblyReference.Assembly);
        
        services.AddScopedAsMatchingInterfaces(Persistence.AssemblyReference.Assembly);
    }
}