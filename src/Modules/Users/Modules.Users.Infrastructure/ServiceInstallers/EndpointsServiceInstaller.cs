using Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Modules.Users.Infrastructure.ServiceInstallers;

/// <summary>
/// Represents the users module endpoints service installer.
/// </summary>
internal sealed class EndpointsServiceInstaller : IServiceInstaller
{
    /// <summary>
    /// Registers MVC controllers from the module's Endpoints assembly into the application's service collection.
    /// </summary>
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers().AddApplicationPart(Endpoints.AssemblyReference.Assembly);
    }
}