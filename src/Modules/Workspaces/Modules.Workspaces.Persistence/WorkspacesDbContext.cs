using Microsoft.EntityFrameworkCore;
using Modules.Workspaces.Persistence.Constants;

namespace Modules.Workspaces.Persistence;

/// <summary>
/// Represents the workspaces module database context.
/// </summary>
public sealed class WorkspacesDbContext(DbContextOptions<WorkspacesDbContext> options) :DbContext(options)
{
    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Workspaces);
        
        modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
    }
}
