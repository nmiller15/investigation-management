using System.Data;
using Investigations.Models.Data;

namespace Investigations.Infrastructure.Data.Parsers;

public class IntParser : ISqlDataParser<int>
{
    public int Parse(IDataReader reader)
    {
        return reader.IsDBNull(0) ? -1 : reader.GetInt32(0);
    }
}
