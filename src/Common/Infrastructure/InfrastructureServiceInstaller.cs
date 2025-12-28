using Application.Time;
using Infrastructure.Configuration;
using Infrastructure.Time;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

/// <summary>
/// Represents the infrastructure service installer.
/// </summary>
public class InfrastructureServiceInstaller : IServiceInstaller
{
    /// <inheritdoc />
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ISystemTime, SystemTime>();
    }
}
