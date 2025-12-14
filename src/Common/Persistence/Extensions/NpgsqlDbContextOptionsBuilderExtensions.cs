using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using Persistence.Constants;

namespace Persistence.Extensions;

/// <summary>
/// Contains extension method for the <see cref="NpgsqlDbContextOptionsBuilder"/> class.
/// </summary>
public static class NpgsqlDbContextOptionsBuilderExtensions
{
    /// <summary>
    /// Configures the migration history table to live in the specified schema.
    /// </summary>
    /// <param name="dbContextOptionsBuilder">The database context options builder.</param>
    /// <param name="schema">The schema.</param>
    /// <summary>
    /// Configure the migrations history table name to use the specified schema.
    /// </summary>
    /// <param name="dbContextOptionsBuilder">The Npgsql DbContext options builder to configure.</param>
    /// <param name="schema">The schema in which the migrations history table will be created.</param>
    /// <returns>The same database context options builder.</returns>
    public static NpgsqlDbContextOptionsBuilder WithMigrationHistoryTableInSchema(
        this NpgsqlDbContextOptionsBuilder dbContextOptionsBuilder,
        string schema
    )
    {
        dbContextOptionsBuilder.MigrationsHistoryTable(TableNames.MigrationHistory, schema);
        
        return dbContextOptionsBuilder;
    }
}
