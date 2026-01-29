using System.Data;

namespace Investigations.Infrastructure.Data.Extensions;

public static class DataReaderExtensions
{
    public static int ParseInt32(this IDataReader reader, string columnName)
    {
        int ordinal = reader.GetOrdinal(columnName);
        return reader.IsDBNull(ordinal) ? -1 : reader.GetInt32(ordinal);
    }

    public static string ParseString(this IDataReader reader, string columnName)
    {
        int ordinal = reader.GetOrdinal(columnName);
        return reader.IsDBNull(ordinal) ? string.Empty : reader.GetString(ordinal);
    }

    public static DateTime ParseDateTime(this IDataReader reader, string columnName)
    {
        int ordinal = reader.GetOrdinal(columnName);
        return reader.IsDBNull(ordinal) ? DateTime.MinValue : reader.GetDateTime(ordinal);
    }
}
