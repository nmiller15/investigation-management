using System.Data;
using Investigations.Configuration;
using Investigations.Features.Shared;
using Investigations.Infrastructure.Data;
using Investigations.Infrastructure.Data.Extensions;
using Investigations.Infrastructure.Data.Parsers;
using Investigations.Models;
using Investigations.Models.Data;
using Serilog;

namespace Investigations.Features.Cases;

public class ListCases
{
    public class Query
    {
        public string SortColumn { get; set; } = "CaseNumber";
        public string SortDirection { get; set; } = "ASC";
        public bool ShowClosedCases { get; set; } = false;

        public class Result
        {
            public List<CaseRow> Cases { get; set; } = [];
        }
    }

    public class Handler(IConnectionStrings connectionStrings)
    {
        public async Task<MethodResponse<Query.Result>> Handle(Query query)
        {
            Log.Debug("Handling ListCases query with SortColumn: {SortColumn}, SortDirection: {SortDirection}, ShowClosedCases: {ShowClosedCases}",
                query.SortColumn, query.SortDirection, query.ShowClosedCases);
            try
            {
                var dcs = new DataCallSettings()
                {
                    ConnectionString = connectionStrings.DefaultConnection,
                    SqlQuery = $"""
                        SELECT 
                            case_key,
                            case_number,
                            is_active,
                            subject_key,
                            subject_first_name,
                            subject_last_name,
                            client_key,
                            client_name,
                            date_of_referral,
                            case_type_code
                        FROM v_cases
                        WHERE (@include_closed_cases = TRUE or is_active = TRUE)
                        ORDER BY
                            CASE WHEN @sort_direction = 'asc' THEN {SortColumn(query.SortColumn)} END ASC,
                            CASE WHEN @sort_direction = 'desc' THEN {SortColumn(query.SortColumn)} END DESC,
                            case_number ASC;
                        """,
                };

                dcs.AddParameter("sort_direction", query.SortDirection.ToLower());
                dcs.AddParameter("include_closed_cases", query.ShowClosedCases);

                var cases = await NpgsqlDataProvider.ExecuteRaw(dcs, new CaseRowParser());

                return MethodResponse<Query.Result>.Success(new Query.Result
                {
                    Cases = cases
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving cases.");
                return MethodResponse<Query.Result>.Failure("An error occurred while retrieving cases. Please try again later.");
            }
        }

        public string SortColumn(string sortColumn)
        {
            return sortColumn.ToLower() switch
            {
                "casenumber" => "case_number",
                "subjectname" => "subject_last_name",
                "clientname" => "client_name",
                "dateofreferral" => "date_of_referral",
                "casetypecode" => "case_type_code",
                _ => "case_number"
            };
        }
    }

    public record CaseRow
    {
        public int CaseKey { get; set; }
        public string CaseNumber { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int SubjectKey { get; set; }
        public string SubjectFirstName { get; set; } = string.Empty;
        public string SubjectLastName { get; set; } = string.Empty;
        public int ClientKey { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public DateTime DateOfReferral { get; set; }
        public string CaseTypeCode { get; set; } = string.Empty;
    }

    public class CaseRowParser : ISqlDataParser<CaseRow>
    {
        public CaseRow Parse(IDataReader reader)
        {
            return new CaseRow
            {
                CaseKey = reader.ParseInt32("case_key"),
                CaseNumber = reader.ParseString("case_number"),
                IsActive = reader.ParseBool("is_active"),
                SubjectKey = reader.ParseInt32("subject_key"),
                SubjectFirstName = reader.ParseString("subject_first_name"),
                SubjectLastName = reader.ParseString("subject_last_name"),
                ClientKey = reader.ParseInt32("client_key"),
                ClientName = reader.ParseString("client_name"),
                DateOfReferral = reader.ParseDateTime("date_of_referral"),
                CaseTypeCode = reader.ParseString("case_type_code")
            };
        }
    }
}

