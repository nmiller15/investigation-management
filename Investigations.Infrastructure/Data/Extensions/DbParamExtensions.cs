using Investigations.Models.Data;
using Npgsql;

namespace Investigations.Infrastructure.Data.Extensions;

public static class DbParamExtensions
{
    public static NpgsqlParameter ToNpgsql(this DbParam p)
    {
        var param = new NpgsqlParameter(p.Name, p.Value ?? DBNull.Value)
        {
            Direction = p.Direction,
        };

        if (p.Type.HasValue)
            param.DbType = p.Type.Value;

        return param;
    }
}
