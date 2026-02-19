using System.Data;
using Investigations.Infrastructure.Data.Extensions;
using Investigations.Models;

namespace Investigations.Infrastructure.Data.Parsers;

public class AppTaskParser : BaseAuditModelParser<AppTask>
{
    public override AppTask Parse(IDataReader reader)
    {
        base.Parse(reader);

        Model.TaskKey = reader.ParseInt32("task_key");
        Model.TaskName = reader.ParseString("task_name");
        Model.TaskDescription = reader.ParseString("task_description");
        Model.CaseKey = reader.ParseInt32("case_key");
        Model.CaseNumber = reader.ParseString("case_number");
        Model.CaseTypeShortDescription = reader.ParseString("case_type_short_description");
        Model.CaseTypeDescription = reader.ParseString("case_type_description");
        Model.SubjectFirstName = reader.ParseString("subject_first_name");
        Model.SubjectLastName = reader.ParseString("subject_last_name");
        Model.AssignedToUserKey = reader.ParseInt32("assigned_to_user_key");
        Model.AssignedToFirstName = reader.ParseString("assigned_to_first_name");
        Model.AssignedToLastName = reader.ParseString("assigned_to_last_name");
        Model.ReminderDate = reader.ParseDateTime("reminder_date");
        Model.DueDate = reader.ParseDateTime("due_date");

        return Model;
    }
}
