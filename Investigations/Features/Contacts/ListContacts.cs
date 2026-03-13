using System.Data;
using Investigations.Configuration;
using Investigations.Infrastructure.Data;
using Investigations.Infrastructure.Data.Extensions;
using Investigations.Models;
using Investigations.Models.Data;

namespace Investigations.Features.Contacts;

public class ListContacts
{
    public record Query
    {
        public string SortColumn { get; set; } = "LastName";
        public string SortDirection { get; set; } = "ASC";

        public record Result
        {
            public List<ContactRow> Contacts { get; set; } = [];
        }

        public bool Validate(out string validationError)
        {
            var validSortColumns = new[] { "FirstName", "LastName", "Email" };
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
            if (!req.Validate(out var validationError))
            {
                return MethodResponse<Query.Result>.Failure(validationError);
            }

            var dcs = new DataCallSettings()
            {
                ConnectionString = connectionStrings.DefaultConnection,
                SqlQuery = $"""
                    SELECT 
                        contact_key,
                        first_name,
                        last_name,
                        email
                    FROM v_contacts
                    ORDER BY 
                        CASE WHEN @sort_direction = 'asc' THEN {ToSqlSortColumn(req.SortColumn)} END ASC,
                        CASE WHEN @sort_direction = 'desc' THEN {ToSqlSortColumn(req.SortColumn)} END DESC;
                    """
            };

            dcs.AddParameter("@sort_direction", req.SortDirection.ToLower());

            var results = await NpgsqlDataProvider.ExecuteRaw(dcs, new ContactRowParser());

            return MethodResponse<Query.Result>.Success(new Query.Result
            {
                Contacts = results
            });
        }

        private static string ToSqlSortColumn(string sortColumn)
        {
            return sortColumn.ToLower() switch
            {
                "firstname" => "first_name",
                "lastname" => "last_name",
                "email" => "email",
                _ => throw new ArgumentException("Invalid sort column.")
            };
        }
    }

    public class ContactRow
    {
        public int ContactKey { get; set; } = 0;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class ContactRowParser : ISqlDataParser<ContactRow>
    {
        public ContactRow Parse(IDataReader reader)
        {
            return new ContactRow
            {
                ContactKey = reader.ParseInt32("contact_key"),
                FirstName = reader.ParseString("first_name"),
                LastName = reader.ParseString("last_name"),
                Email = reader.ParseString("email")
            };
        }
    }
}
