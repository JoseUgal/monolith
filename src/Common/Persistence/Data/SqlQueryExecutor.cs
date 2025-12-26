using Application.Data;
using Application.ServiceLifetimes;
using Dapper;

namespace Persistence.Data;

/// <summary>
/// Represents the SQL query executor.
/// </summary>
internal sealed class SqlQueryExecutor(ISqlConnectionFactory sqlConnectionFactory) : ISqlQueryExecutor, ITransient
{
    /// <inheritdoc />
    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null)
    {
        return await sqlConnectionFactory.GetOpenConnection().QueryAsync<T>(
            sql, 
            parameters
        );
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TResult>> QueryAsync<T1, T2, TResult>(string sql, Func<T1, T2, TResult> map, object? parameters = null, string splitOn = "Id")
    {
        return await sqlConnectionFactory.GetOpenConnection().QueryAsync<T1, T2, TResult>(
            sql,
            map,
            parameters,
            splitOn: splitOn
        );
    }

    /// <inheritdoc />
    public async Task<T?> FirstOrDefaultAsync<T>(string sql, object? parameters = null)
    {
        return await sqlConnectionFactory.GetOpenConnection().QueryFirstOrDefaultAsync<T>(
            sql,
            parameters
        );
    }

    /// <inheritdoc />
    public async Task ExecuteAsync(string sql, object? parameters = null)
    {
        await sqlConnectionFactory.GetOpenConnection().ExecuteAsync(
            sql,
            parameters
        );
    }

    /// <inheritdoc />
    public async Task<T> ExecuteScalarAsync<T>(string sql, object? parameters = null)
    {
        T? result = await sqlConnectionFactory.GetOpenConnection().ExecuteScalarAsync<T>(
            sql,
            parameters
        );

        if (result == null)
        {
            throw new InvalidOperationException(
                "The scalar query returned a null result."
            );
        }
        
        return result;
    }
}
