using Investigations.Configuration;
using Investigations.Infrastructure.Data;
using Investigations.Infrastructure.Data.Parsers;
using Investigations.Models;

namespace Investigations.Features.Cases;

public class ViewCase
{
    public record Query
    {
        public int CaseKey { get; set; }

        public record Result
        {
            public Case Case { get; set; } = new Case();
        }
    }


    public class Handler(IConnectionStrings connectionStrings)
    {
        private readonly IConnectionStrings _connectionStrings = connectionStrings;

        public async Task<MethodResponse<Query.Result>> Handle(Query query)
        {
            var caseData = await GetCase(query.CaseKey);

            return caseData.CaseKey > 0
                ? MethodResponse<Query.Result>.Success(new Query.Result { Case = caseData })
                : MethodResponse<Query.Result>.Failure("Case not found.");
        }

        private async Task<Case> GetCase(int caseKey)
        {
            var dcs = new DataCallSettings()
            {
                ConnectionString = _connectionStrings.DefaultConnection,
                FunctionName = "get_case_by_key",
            };
            dcs.AddParameter("case_key", caseKey);

            var records = await NpgsqlDataProvider.ExecuteFunction(dcs, new CaseParser());

            return records.FirstOrDefault() ?? new Case();
        }
    }
}
