using System.Data;
using Investigations.Models.Data;
using Npgsql;
using NpgsqlTypes;

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
        {
            if (p.Type.Value == DbType.DateTime)
            {
                param.DataTypeName = null;
                param.NpgsqlDbType = NpgsqlDbType.Timestamp; // force timestamp
            }
            else
            {
                param.DbType = p.Type.Value;
            }
        }

        return param;
    }
}
