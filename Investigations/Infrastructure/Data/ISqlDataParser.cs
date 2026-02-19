using System.Data;

namespace Investigations.Models.Data;

public interface ISqlDataParser<T>
{
    public T Parse(IDataReader reader);
}
