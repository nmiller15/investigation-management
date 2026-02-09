using System.Data;
using Investigations.Infrastructure.Data.Extensions;
using Investigations.Models.Cases;

namespace Investigations.Infrastructure.Data.Parsers;

public class CaseParser : BaseAuditModelParser<Case>
{
    public override Case Parse(IDataReader reader)
    {
        base.Parse(reader);

        Model.CaseKey = reader.ParseInt32("case_key");
        Model.CaseNumber = reader.ParseString("case_number");
        Model.IsActive = reader.ParseBool("is_active");
        Model.SubjectKey = reader.ParseInt32("subject_key");
        Model.SubjectFirstName = reader.ParseString("subject_first_name");
        Model.SubjectLastName = reader.ParseString("subject_last_name");
        Model.ClientKey = reader.ParseInt32("client_key");
        Model.ClientName = reader.ParseString("client_name");
        Model.DateOfReferral = reader.ParseDateTime("date_of_referral");
        Model.CaseTypeCodeKey = reader.ParseInt32("case_type_code_key");
        Model.CaseTypeCode = reader.ParseString("case_type_code");
        Model.CaseTypeShortDescription = reader.ParseString("case_type_short_description");
        Model.CaseTypeDescription = reader.ParseString("case_type_description");
        Model.Synopsis = reader.ParseString("synopsis");

        return Model;
    }
}

