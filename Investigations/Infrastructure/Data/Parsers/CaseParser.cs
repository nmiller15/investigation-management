using System.Data;
using Investigations.Infrastructure.Data.Extensions;
using Investigations.Models;

namespace Investigations.Infrastructure.Data.Parsers;

public class CaseParser : BaseAuditModelParser<Case>
{
    public override Case Parse(IDataReader reader)
    {
        base.Parse(reader);

        Model.CaseKey = reader.ParseInt32("case_key");
        Model.CaseNumber = reader.ParseString("case_number");
        Model.IsActive = reader.ParseBool("is_active");
        Model.Subject.SubjectKey = reader.ParseInt32("subject_key");
        Model.Subject.FirstName = reader.ParseString("subject_first_name");
        Model.Subject.LastName = reader.ParseString("subject_last_name");
        Model.Client.ClientKey = reader.ParseInt32("client_key");
        Model.Client.ClientName = reader.ParseString("client_name");
        Model.DateOfReferral = reader.ParseDateTime("date_of_referral");
        Model.Type = (Case.Types)reader.ParseInt32("case_type_code_key");
        Model.Synopsis = reader.ParseString("synopsis");

        return Model;
    }
}
