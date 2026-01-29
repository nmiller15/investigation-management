using System.Data;
using Investigations.Infrastructure.Data.Extensions;
using Investigations.Models;

namespace Investigations.Infrastructure.Data.Parsers;

public class UserParser : BaseAuditModelParser<User>
{
    public UserParser() : base() { }

    public override User Parse(IDataReader reader)
    {
        base.Parse(reader);

        Model.UserKey = reader.ParseInt32("user_key");
        Model.FirstName = reader.ParseString("first_name");
        Model.LastName = reader.ParseString("last_name");
        Model.Email = reader.ParseString("email");
        Model.Birthdate = reader.ParseDateTime("birthdate");
        Model.RoleCodeKey = reader.ParseInt32("role_code_key");
        Model.RoleDescription = reader.ParseString("role_description");

        return Model;
    }

}
