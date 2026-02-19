using Investigations.Configuration;
using Investigations.Infrastructure.Data.Parsers;
using Investigations.Models;

namespace Investigations.Infrastructure.Data.Repositories;

public class CaseRepository(IConnectionStrings connectionStrings) : BaseSqlRepository(connectionStrings)
{
    public async Task<List<Case>> GetCases()
    {
        var dcs = GetFunctionCallDcsInstance("get_cases");
        var results = await NpgsqlDataProvider.ExecuteFunction(dcs, new CaseParser());
        return results ?? [];
    }

    public async Task<Case> GetCase(int caseKey)
    {
        var dcs = GetFunctionCallDcsInstance("get_case_by_key");
        AddCaseKeyParameter(caseKey, dcs);
        var results = await NpgsqlDataProvider.ExecuteFunction(dcs, new CaseParser());
        return results?.FirstOrDefault() ?? new();
    }

    public async Task<int> AddCase(Case case_, int insertedByUserKey = 100)
    {
        var dcs = GetFunctionCallDcsInstance("add_case");
        AddCaseKeyParameter(case_.CaseKey, dcs);
        AddCaseParameters(case_, dcs);
        dcs.AddParameter("p_inserted_by_user_key", insertedByUserKey);
        var results = await NpgsqlDataProvider.ExecuteFunctionScalar(dcs, new IntParser());
        return results;
    }

    public async Task<int> UpdateCase(Case case_, int updatedByUserKey = 100)
    {
        var dcs = GetFunctionCallDcsInstance("update_case");
        AddCaseKeyParameter(case_.CaseKey, dcs);
        AddCaseParameters(case_, dcs);
        dcs.AddParameter("p_updated_by_user_key", updatedByUserKey);
        var results = await NpgsqlDataProvider.ExecuteFunctionScalar(dcs, new IntParser());
        return results;
    }

    public async Task<int> DeleteCase(int caseKey)
    {
        var dcs = GetFunctionCallDcsInstance("delete_case");
        AddCaseKeyParameter(caseKey, dcs);
        var result = await NpgsqlDataProvider.ExecuteFunctionScalar(dcs, new IntParser());
        return result;
    }

    private static void AddCaseKeyParameter(int caseKey, DataCallSettings dcs)
    {
        dcs.AddParameter("p_case_key", caseKey);
    }

    private static void AddCaseParameters(Case case_, DataCallSettings dcs)
    {
        dcs.AddParameter("p_case_key", case_.CaseKey);
        dcs.AddParameter("p_case_number", case_.CaseNumber);
        dcs.AddParameter("p_is_active", case_.IsActive);
        dcs.AddParameter("p_subject_key", case_.SubjectKey);
        dcs.AddParameter("p_client_key", case_.ClientKey);
        dcs.AddParameter("p_date_of_referral", case_.DateOfReferral);
        dcs.AddParameter("p_case_type_code_key", case_.CaseTypeCodeKey);
        dcs.AddParameter("p_synopsis", case_.Synopsis);
    }
}
