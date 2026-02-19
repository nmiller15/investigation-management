using System.Data;
using Investigations.Infrastructure.Data.Extensions;
using Investigations.Models;

namespace Investigations.Infrastructure.Data.Parsers;

public class AddressParser : BaseAuditModelParser<Address>
{
    public override Address Parse(IDataReader reader)
    {
        base.Parse(reader);

        Model.AddressKey = reader.ParseInt32("address_key");
        Model.LineOne = reader.ParseString("line_one");
        Model.LineTwo = reader.ParseString("line_two");
        Model.City = reader.ParseString("city");
        Model.StateCodeKey = reader.ParseInt32("state_code_key");
        Model.State = reader.ParseString("state");
        Model.StateAbbreviation = reader.ParseString("state_abbreviation");
        Model.CountryCodeKey = reader.ParseInt32("country_code_key");
        Model.Country = reader.ParseString("country");
        Model.CountryAbbreviation = reader.ParseString("country_abbreviation");
        Model.Zip = reader.ParseString("zip");

        return Model;
    }
}
