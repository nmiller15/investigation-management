using System.Data;
using Investigations.Infrastructure.Data.Extensions;
using Investigations.Models;

namespace Investigations.Infrastructure.Data.Parsers;

public class ContactParser : BaseAuditModelParser<Contact>
{
    public override Contact Parse(IDataReader reader)
    {
        base.Parse(reader);

        Model.ContactKey = reader.ParseInt32("ContactKey");
        Model.FirstName = reader.ParseString("FirstName");
        Model.LastName = reader.ParseString("LastName");
        Model.Email = reader.ParseString("Email");
        Model.MobilePhone = reader.ParseString("MobilePhone");
        Model.WorkPhone = reader.ParseString("WorkPhone");
        Model.HomePhone = reader.ParseString("HomePhone");
        Model.Notes = reader.ParseString("Notes");

        return Model;
    }
}
