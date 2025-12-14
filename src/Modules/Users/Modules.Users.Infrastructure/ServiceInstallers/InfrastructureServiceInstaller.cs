using Application.Time;
using Infrastructure.Configuration;
using Infrastructure.Time;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Modules.Users.Infrastructure.ServiceInstallers;

/// <summary>
/// Represents the users module infrastructure service installer.
/// </summary>
internal sealed class InfrastructureServiceInstaller : IServiceInstaller
{
    /// <summary>
    /// Registers infrastructure services for the Users module by adding a transient mapping from <c>ISystemTime</c> to <c>SystemTime</c> if no existing registration is present.
    /// </summary>
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.TryAddTransient<ISystemTime, SystemTime>();
    }
}