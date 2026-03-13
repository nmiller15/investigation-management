using System.Data;
using Investigations.Configuration;
using Investigations.Infrastructure.Data;
using Investigations.Infrastructure.Data.Extensions;
using Investigations.Models;
using Investigations.Models.Data;
using Serilog;

namespace Investigations.Features.Clients;

public class ListClients
{
    public record Query
    {
        public string SortColumn { get; set; } = "ClientName";
        public string SortDirection { get; set; } = "ASC";

        public record Result
        {
            public List<ClientRow> Clients { get; set; } = [];
        }

        public bool Validate(out string validationError)
        {
            var validSortColumns = new[] { "ClientName", "PrimaryContactLastName" };
            if (!validSortColumns.Select(c => c.ToLower()).Contains(SortColumn.ToLower()))
            {
                validationError = $"Invalid sort column. Valid options are: {string.Join(", ", validSortColumns)}.";
                return false;
            }

            var validSortDirections = new[] { "ASC", "DESC" };
            if (!validSortDirections.Contains(SortDirection.ToUpper()))
            {
                validationError = $"Invalid sort direction. Valid options are: {string.Join(", ", validSortDirections)}.";
                return false;
            }

            validationError = string.Empty;
            return true;
        }
    }

    public class Handler(IConnectionStrings connectionStrings)
    {
        public async Task<MethodResponse<Query.Result>> Handle(Query req)
        {
            Log.Debug("Handling ListClients query with SortColumn: {SortColumn}, SortDirection: {SortDirection}",
                req.SortColumn, req.SortDirection);

            if (!req.Validate(out var validationError))
            {
                return MethodResponse<Query.Result>.Failure(validationError);
            }

            var dcs = new DataCallSettings()
            {
                ConnectionString = connectionStrings.DefaultConnection,
                SqlQuery = $"""
                    SELECT 
                        vc.client_key,
                        vc.client_name,
                        vc.primary_contact_key,
                        vc.primary_contact_first_name,
                        vc.primary_contact_last_name
                    FROM v_clients vc
                    ORDER BY 
                        CASE WHEN @sort_direction = 'asc' THEN vc.{ToSqlSortColumn(req.SortColumn)} END ASC,
                        CASE WHEN @sort_direction = 'desc' THEN vc.{ToSqlSortColumn(req.SortColumn)} END DESC;
                """
            };

            dcs.AddParameter("@sort_direction", req.SortDirection.ToLower());

            var results = await NpgsqlDataProvider.ExecuteRaw(dcs, new ClientRowParser());
            return MethodResponse<Query.Result>.Success(new Query.Result()
            {
                Clients = results
            });
        }

        private static string ToSqlSortColumn(string sortColumn)
        {
            return sortColumn.ToLower() switch
            {
                "clientname" => "client_name",
                "primarycontactlastname" => "primary_contact_last_name",
                _ => throw new ArgumentException("Invalid sort column.")
            };
        }
    }

    public record ClientRow
    {
        public int ClientKey { get; set; } = 0;
        public string ClientName { get; set; } = string.Empty;
        public int PrimaryContactKey { get; set; }
        public string PrimaryContactFirstName { get; set; }
        public string PrimaryContactLastName { get; set; }
    }

    public class ClientRowParser : ISqlDataParser<ClientRow>
    {
        public ClientRow Parse(IDataReader reader)
        {
            return new ClientRow
            {
                ClientKey = reader.ParseInt32("client_key"),
                ClientName = reader.ParseString("client_name"),
                PrimaryContactKey = reader.ParseInt32("primary_contact_key"),
                PrimaryContactFirstName = reader.ParseString("primary_contact_first_name"),
                PrimaryContactLastName = reader.ParseString("primary_contact_last_name")
            };
        }
    }

    internal static async Task Handle(Query query)
    {
        throw new NotImplementedException();
    }
}
