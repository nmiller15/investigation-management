using System.Data;
using Investigations.Configuration;
using Investigations.Infrastructure.Data;
using Investigations.Infrastructure.Data.Extensions;
using Investigations.Models;
using Investigations.Models.Data;
using Serilog;

namespace Investigations.Features.Cases;

public class ViewCase
{
    public record Query
    {
        public int CaseKey { get; set; }

        public record Result
        {
            public CaseRow Case { get; set; } = new CaseRow();
            public List<TaskRow> Tasks { get; set; } = [];
        }
    }

    public class Handler(IConnectionStrings connectionStrings)
    {
        public async Task<MethodResponse<Query.Result>> Handle(Query query)
        {
            Log.Debug("Handling ViewCase query for CaseKey: {CaseKey}", query.CaseKey);
            var dcs = new DataCallSettings
            {
                ConnectionString = connectionStrings.DefaultConnection,
                SqlQuery = """
                    SELECT 
                        case_key, case_number, is_active, subject_key, subject_first_name, subject_last_name, client_key, client_name, 
                        case_type_code, case_type_description, date_of_referral, synopsis, updated_by_user_key, updated_by_user_first_name,
                        updated_by_user_last_name, updated_datetime, inserted_by_user_key, inserted_by_user_first_name,
                        inserted_by_user_last_name, inserted_datetime
                    FROM v_cases
                    WHERE case_key = @case_key;
                    """,
            };

            dcs.AddParameter("case_key", query.CaseKey);

            var caseRows = await NpgsqlDataProvider.ExecuteRaw(dcs, new CaseRowParser());

            dcs.SqlQuery = """
                SELECT 
                    task_key, is_completed, task_name, task_description, assigned_to_user_key, assigned_to_first_name,
                    assigned_to_last_name, due_date, inserted_datetime
                FROM v_tasks
                WHERE case_key = @case_key
                ORDER BY due_date DESC, inserted_datetime DESC;
                """;

            var taskRows = await NpgsqlDataProvider.ExecuteRaw(dcs, new TaskRowParser());


            return MethodResponse<Query.Result>.Success(new Query.Result
            {
                Case = caseRows.FirstOrDefault() ?? new CaseRow(),
                Tasks = taskRows,
            });
        }
    }

    public class CaseRow
    {
        public int CaseKey { get; set; }
        public string CaseNumber { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int SubjectKey { get; set; }
        public string SubjectFirstName { get; set; } = string.Empty;
        public string SubjectLastName { get; set; } = string.Empty;
        public int ClientKey { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public string CaseTypeCode { get; set; } = string.Empty;
        public string CaseTypeDescription { get; set; } = string.Empty;
        public DateTime DateOfReferral { get; set; }
        public string Synopsis { get; set; } = string.Empty;
        public int UpdatedByUserKey { get; set; }
        public string UpdatedByFirstName { get; set; } = string.Empty;
        public string UpdatedByLastName { get; set; } = string.Empty;
        public DateTime UpdatedOnDateTime { get; set; }
        public int InsertedByUserKey { get; set; }
        public string InsertedByFirstName { get; set; } = string.Empty;
        public string InsertedByLastName { get; set; } = string.Empty;
        public DateTime InsertedOnDateTime { get; set; }
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
                CaseTypeCode = reader.ParseString("case_type_code"),
                CaseTypeDescription = reader.ParseString("case_type_description"),
                DateOfReferral = reader.ParseDateTime("date_of_referral"),
                Synopsis = reader.ParseString("synopsis"),
                UpdatedByUserKey = reader.ParseInt32("updated_by_user_key"),
                UpdatedByFirstName = reader.ParseString("updated_by_user_first_name"),
                UpdatedByLastName = reader.ParseString("updated_by_user_last_name"),
                UpdatedOnDateTime = reader.ParseDateTime("updated_datetime"),
                InsertedByUserKey = reader.ParseInt32("inserted_by_user_key"),
                InsertedByFirstName = reader.ParseString("inserted_by_user_first_name"),
                InsertedByLastName = reader.ParseString("inserted_by_user_last_name"),
                InsertedOnDateTime = reader.ParseDateTime("inserted_datetime"),
            };
        }
    }

    public class TaskRow
    {
        public int TaskKey { get; set; }
        public bool IsCompleted { get; set; }
        public string TaskName { get; set; } = string.Empty;
        public string TaskDescription { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        public int AssignedToUserKey { get; set; }
        public string AssignedToFirstName { get; set; } = string.Empty;
        public string AssignedToLastName { get; set; } = string.Empty;
        public DateTime InsertedDateTime { get; set; }
    }

    public class TaskRowParser : ISqlDataParser<TaskRow>
    {
        public TaskRow Parse(IDataReader reader)
        {
            return new TaskRow
            {
                TaskKey = reader.ParseInt32("task_key"),
                IsCompleted = reader.ParseBool("is_completed"),
                TaskName = reader.ParseString("task_name"),
                TaskDescription = reader.ParseString("task_description"),
                DueDate = reader.ParseDateTime("due_date").Equals(DateTime.MinValue)
                    ? null
                    : reader.ParseDateTime("due_date"),
                AssignedToUserKey = reader.ParseInt32("assigned_to_user_key"),
                AssignedToFirstName = reader.ParseString("assigned_to_first_name"),
                AssignedToLastName = reader.ParseString("assigned_to_last_name"),
                InsertedDateTime = reader.ParseDateTime("inserted_datetime"),
            };
        }
    }
}
