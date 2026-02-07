using System.Data;
using Investigations.Models.Data;

namespace Investigations.Infrastructure.Data.Parsers;

public class StringParser : ISqlDataParser<string>
{
    public string Parse(IDataReader reader)
    {
        return reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
    }
}

