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
    /// <summary>
    /// Performs database migration for the UsersDbContext when the application is running in the Development environment.
    /// </summary>
    /// <param name="stoppingToken">Cancellation token that signals the host is stopping and should be observed to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> that completes after the environment check and, if in Development, the migration has been attempted.</returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!environment.IsDevelopment())
        {
            return;
        }
        
        using IServiceScope scope = serviceProvider.CreateScope();

        await MigrateDatabaseAsync<UsersDbContext>(scope, stoppingToken);
    }

    /// <summary>
    /// Applies any pending Entity Framework Core migrations for the specified DbContext resolved from the provided scope.
    /// </summary>
    /// <typeparam name="TDbContext">The DbContext type to migrate.</typeparam>
    /// <param name="scope">The service scope used to resolve the requested <typeparamref name="TDbContext"/>.</param>
    /// <param name="cancellationToken">Token to cancel the migration operation.</param>
    private static async Task MigrateDatabaseAsync<TDbContext>(IServiceScope scope, CancellationToken cancellationToken) where TDbContext : DbContext
    {
        TDbContext dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();
        
        await dbContext.Database.MigrateAsync(cancellationToken);
    }
}