using System.Data;

namespace Investigations.Models.Data;

public interface ISqlDataParser<T> where T : new()
{
    public T Parse(IDataReader reader);
}
