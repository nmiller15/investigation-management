using System.Data;
using Investigations.Infrastructure.Data.Extensions;
using Investigations.Models;
using Investigations.Models.Data;

namespace Investigations.Infrastructure.Data.Parsers;

public class BaseAuditModelParser<T> : ISqlDataParser<T> where T : BaseAuditModel, new()
{
    protected T Model = new();

    public virtual T Parse(IDataReader reader)
    {
        Model = new T
        {
            InsertedByUserKey = reader.ParseInt32("inserted_by_user_key"),
            InsertedByFirstName = reader.ParseString("inserted_by_user_first_name"),
            InsertedByLastName = reader.ParseString("inserted_by_user_last_name"),
            InsertedDateTime = reader.ParseDateTime("inserted_datetime"),
            UpdatedByUserKey = reader.ParseInt32("updated_by_user_key"),
            UpdatedByFirstName = reader.ParseString("updated_by_user_first_name"),
            UpdatedByLastName = reader.ParseString("updated_by_user_last_name"),
            UpdatedDateTime = reader.ParseDateTime("updated_datetime"),
        };
        return Model;
    }
}
