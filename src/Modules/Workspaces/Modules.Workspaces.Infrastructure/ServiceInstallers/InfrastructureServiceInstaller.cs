using Application.Time;
using Infrastructure.Configuration;
using Infrastructure.Time;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Modules.Workspaces.Infrastructure.ServiceInstallers;

/// <summary>
/// Represents the workspaces module infrastructure service installer.
/// </summary>
internal sealed class InfrastructureServiceInstaller : IServiceInstaller
{
    /// <inheritdoc />
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.TryAddTransient<ISystemTime, SystemTime>();
    }
}
