using Microsoft.EntityFrameworkCore;
using Modules.Users.Persistence;

namespace App.StartupTasks;

/// <summary>
/// Represents the startup task for migrating the database in the development environment only.
/// </summary>
/// <param name="environment">The environment.</param>
/// <param name="serviceProvider">The service provider.</param>
internal sealed class MigrateDatabaseStartupTask(
    IHostEnvironment environment,
    IServiceProvider serviceProvider
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!environment.IsDevelopment())
        {
            return;
        }
        
        using IServiceScope scope = serviceProvider.CreateScope();

        await MigrateDatabaseAsync<UsersDbContext>(scope, stoppingToken);
    }

    private static async Task MigrateDatabaseAsync<TDbContext>(IServiceScope scope, CancellationToken cancellationToken) where TDbContext : DbContext
    {
        TDbContext dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();
        
        await dbContext.Database.MigrateAsync(cancellationToken);
    }
}
