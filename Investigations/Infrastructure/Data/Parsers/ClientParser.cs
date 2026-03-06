using System.Data;
using Investigations.Infrastructure.Data.Extensions;
using Investigations.Models;

namespace Investigations.Infrastructure.Data.Parsers;

public class ClientParser : BaseAuditModelParser<Client>
{
    public override Client Parse(IDataReader reader)
    {
        base.Parse(reader);

        Model.ClientKey = reader.ParseInt32("client_key");
        Model.ClientName = reader.ParseString("client_name");
        Model.PrimaryContact.ContactKey = reader.ParseInt32("primary_contact_key");
        Model.PrimaryContact.FirstName = reader.ParseString("primary_contact_first_name");
        Model.PrimaryContact.LastName = reader.ParseString("primary_contact_last_name");

        return Model;
    }
}
