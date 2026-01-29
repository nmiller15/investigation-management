using System.Data;
using Investigations.Infrastructure.Data.Extensions;
using Investigations.Models.Data;
using Npgsql;

namespace Investigations.Infrastructure.Data;

public static class NpgsqlDataProvider
{
    public async static Task<T> ExecuteFunctionScalar<T>(DataCallSettings dcs, ISqlDataParser<T> parser)
    {
        var functionName = dcs.FunctionName;
        var connectionString = dcs.ConnectionString;
        var parameters = dcs.Parameters
            .Select(p => p.ToNpgsql())
            .ToArray();

        using var conn = new NpgsqlConnection(connectionString);
        conn.Open();

        var sql = $"SELECT {functionName}({string.Join(",", parameters.Select(p => "@" + p.ParameterName))})";
        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddRange(parameters);

        var result = await cmd.ExecuteScalarAsync();
        return result != DBNull.Value ? (T)Convert.ChangeType(result, typeof(T)) : default!;
    }

    public async static Task<List<T>> ExecuteFunction<T>(DataCallSettings dcs, ISqlDataParser<T> parser)
    {
        var results = new List<T>();

        var functionName = dcs.FunctionName;
        var connectionString = dcs.ConnectionString;
        var parameters = dcs.Parameters
            .Select(p => p.ToNpgsql())
            .ToArray();

        using var conn = new NpgsqlConnection(connectionString);

        var sql = $"SELECT * FROM {functionName}({string.Join(",", parameters.Select(p => "@" + p.ParameterName))})";
        var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddRange(parameters);

        conn.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            results.Add(parser.Parse(reader));
        }

        return results;
    }

    public async static Task ExecuteProcedure(DataCallSettings dcs)
    {
        var procedureName = dcs.ProcedureName;
        var connectionString = dcs.ConnectionString;
        var parameters = dcs.Parameters
            .Select(p => p.ToNpgsql())
            .ToArray();

        using var conn = new NpgsqlConnection(connectionString);
        conn.Open();

        using var cmd = new NpgsqlCommand(procedureName, conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddRange(parameters);

        cmd.ExecuteNonQuery();
    }
}
