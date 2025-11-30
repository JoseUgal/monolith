using Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Modules.Users.Persistence;
using Modules.Users.Persistence.Constants;
using Persistence.Extensions;
using Persistence.Interceptors;
using Persistence.Options;

namespace Modules.Users.Infrastructure.ServiceInstallers;

/// <summary>
/// Represents the users module persistence service installer.
/// </summary>
internal class PersistenceServiceInstaller : IServiceInstaller
{
    /// <inheritdoc />
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.TryAddSingleton<UpdateAuditablePropertiesInterceptor>();
        
        services.AddDbContext<UsersDbContext>((serviceProvider, options) =>
            {
                ConnectionStringOptions connectionString = serviceProvider.GetService<IOptions<ConnectionStringOptions>>()!.Value;

                options.UseNpgsql(connectionString, builder => 
                    builder.WithMigrationHistoryTableInSchema(Schemas.Users)
                );

                options.UseSnakeCaseNamingConvention();

                options.AddInterceptors(
                    serviceProvider.GetService<UpdateAuditablePropertiesInterceptor>()!
                );
            }    
        );
    }
}
