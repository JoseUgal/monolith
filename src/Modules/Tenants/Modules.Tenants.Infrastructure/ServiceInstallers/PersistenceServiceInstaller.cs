using Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Modules.Tenants.Persistence;
using Modules.Tenants.Persistence.Constants;
using Persistence.Extensions;
using Persistence.Interceptors;
using Persistence.Options;

namespace Modules.Tenants.Infrastructure.ServiceInstallers;

/// <summary>
/// Represents the tenants module persistence service installer.
/// </summary>
internal class PersistenceServiceInstaller : IServiceInstaller
{
    /// <inheritdoc />
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.TryAddSingleton<UpdateAuditablePropertiesInterceptor>();
        
        services.AddDbContext<TenantsDbContext>((serviceProvider, options) =>
            {
                ConnectionStringOptions connectionString = serviceProvider.GetRequiredService<IOptions<ConnectionStringOptions>>().Value;

                options.UseNpgsql(connectionString, builder => 
                    builder.WithMigrationHistoryTableInSchema(Schemas.Tenants)
                );

                options.UseSnakeCaseNamingConvention();

                options.AddInterceptors(
                    serviceProvider.GetRequiredService<UpdateAuditablePropertiesInterceptor>()!
                );
            }    
        );
    }
}
