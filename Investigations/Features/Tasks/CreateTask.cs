using System.Data;
using Investigations.Configuration;
using Investigations.Infrastructure.Data;
using Investigations.Infrastructure.Data.Extensions;
using Investigations.Infrastructure.Data.Parsers;
using Investigations.Models;
using Investigations.Models.Data;

namespace Investigations.Features.Tasks;

public class CreateTask
{
    public record Query
    {
        public bool CanAssignToOtherUsers { get; set; }
        public bool CanAssignToAnyCase { get; set; }
        public int CurrentUserKey { get; set; }
        public int? SourcedCaseKey { get; set; }

        public record Response
        {
            public List<AssignableUser> AssignableUsers { get; set; } = [];
            public List<AssignableCase> AssignableCases { get; set; } = [];
        }

        public bool Validate(out string? validationMessage)
        {
            validationMessage = null;
            if (!CanAssignToAnyCase && !SourcedCaseKey.HasValue)
            {
                validationMessage = "Must specify a sourced case key when CanAssignToAnyCase is false.";
                return false;
            }

            return true;
        }
    }

    public record Command
    {
        public int CaseKey { get; set; }
        public string TaskName { get; set; } = string.Empty;
        public string TaskDescription { get; set; } = string.Empty;
        public int? AssignedToUserKey { get; set; }
        public DateTime? DueDate { get; set; }
        public int InsertedByUserKey { get; set; }
    }


    public class Handler(IConnectionStrings connectionStrings)
    {
        public async Task<MethodResponse<Query.Response>> Handle(Query query)
        {
            if (!query.Validate(out string? validationMessage))
                return MethodResponse<Query.Response>.Failure(validationMessage ?? "Invalid query parameters.");

            var clause = query.CanAssignToOtherUsers
                        ? $"WHERE role_code_key <> {(int)User.Roles.SystemAdministrator};"
                        : $"WHERE user_key = {query.CurrentUserKey};";

            var dcs = new DataCallSettings
            {
                ConnectionString = connectionStrings.DefaultConnection,
                SqlQuery = $"""
                    SELECT user_key, first_name, last_name, email
                    FROM v_users
                    {clause}
                    """,
            };

            var users = await NpgsqlDataProvider.ExecuteRaw(dcs, new AssignableUserParser());

            var caseDcs = new DataCallSettings
            {
                ConnectionString = connectionStrings.DefaultConnection,
                SqlQuery = $"""
                    SELECT case_key, case_number, subject_first_name, subject_last_name
                    FROM v_cases
                    WHERE is_active = true
                    {(query.CanAssignToAnyCase ? "" : $"AND case_key = {query.SourcedCaseKey}")}
                    ORDER BY case_number DESC;
                    """,
            };

            var cases = await NpgsqlDataProvider.ExecuteRaw(caseDcs, new AssignableCaseParser());

            return MethodResponse<Query.Response>.Success(new Query.Response
            {
                AssignableUsers = users,
                AssignableCases = cases
            });
        }

        public async Task<MethodResponse<bool>> Handle(Command command)
        {
            var dcs = new DataCallSettings
            {
                ConnectionString = connectionStrings.DefaultConnection,
                SqlQuery = """
                    INSERT INTO tasks(case_key, task_name, task_description, assigned_to_user_key, due_date)
                    VALUES(@case_key, @task_name, @task_description, @assigned_to_user_key, @due_date);
                """,
            };

            dcs.AddParameter("case_key", command.CaseKey);
            dcs.AddParameter("task_name", command.TaskName);
            dcs.AddParameter("task_description", command.TaskDescription);
            dcs.AddParameter("assigned_to_user_key", command.AssignedToUserKey, typeof(int?));
            dcs.AddParameter("due_date", command.DueDate, typeof(DateTime?));
            dcs.AddParameter("inserted_by_user_key", command.InsertedByUserKey);

            var result = await NpgsqlDataProvider.ExecuteRaw(dcs, new IntParser());

            return MethodResponse<bool>.Success(true);
        }
    }

    public class AssignableUser
    {
        public int UserKey { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}".Trim();
    }

    public class AssignableUserParser : ISqlDataParser<AssignableUser>
    {
        public AssignableUser Parse(IDataReader reader)
        {
            return new AssignableUser
            {
                UserKey = reader.ParseInt32("user_key"),
                FirstName = reader.ParseString("first_name"),
                LastName = reader.ParseString("last_name"),
                Email = reader.ParseString("email")
            };
        }
    }

    public class AssignableCase
    {
        public int CaseKey { get; set; }
        public string CaseNumber { get; set; } = string.Empty;
        public string SubjectFirstName { get; set; } = string.Empty;
        public string SubjectLastName { get; set; } = string.Empty;
    }

    public class AssignableCaseParser : ISqlDataParser<AssignableCase>
    {
        public AssignableCase Parse(IDataReader reader)
        {
            return new AssignableCase
            {
                CaseKey = reader.ParseInt32("case_key"),
                CaseNumber = reader.ParseString("case_number"),
                SubjectFirstName = reader.ParseString("subject_first_name"),
                SubjectLastName = reader.ParseString("subject_last_name"),
            };
        }
    }
}
