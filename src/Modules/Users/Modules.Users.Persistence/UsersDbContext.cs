using Microsoft.EntityFrameworkCore;
using Modules.Users.Persistence.Constants;

namespace Modules.Users.Persistence;

/// <summary>
/// Represents the users module database context.
/// </summary>
public sealed class UsersDbContext(DbContextOptions<UsersDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Configures the EF Core model: sets the default schema to the users schema and applies entity configurations from the module assembly.
    /// </summary>
    /// <param name="modelBuilder">The builder used to configure entity types, relationships, and mappings.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Users);

        modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
    }
}