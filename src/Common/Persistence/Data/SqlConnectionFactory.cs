using System.Data;
using Application.ServiceLifetimes;
using Microsoft.Extensions.Options;
using Npgsql;
using Persistence.Options;

namespace Persistence.Data;

/// <summary>
/// Represents the SQL connection factory.
/// </summary>
internal sealed class SqlConnectionFactory(IOptions<ConnectionStringOptions> options) : ISqlConnectionFactory, IDisposable, ITransient
{
    private readonly ConnectionStringOptions _connectionString = options.Value;

    private IDbConnection? _connection;

    /// <inheritdoc />
    public IDbConnection GetOpenConnection()
    {
        if ((_connection ??= new NpgsqlConnection(_connectionString)).State != ConnectionState.Open)
        {
            _connection.Open();
        }

        return _connection;
    }
    
    /// <inheritdoc />
    public void Dispose() => _connection?.Dispose();
}
