using Investigations.Configuration;
using Investigations.Infrastructure.Data.Parsers;
using Investigations.Models;

namespace Investigations.Infrastructure.Data.Repositories;

public class AppTaskRepository(IConnectionStrings connectionStrings)
    : BaseSqlRepository(connectionStrings)
{
    private readonly AppTaskParser _appTaskParser = new();
    private readonly IntParser _intParser = new();

    public async Task<List<AppTask>> GetTasks(int assignedToUserKey)
    {
        var dcs = GetFunctionCallDcsInstance("get_tasks");

        var records = await NpgsqlDataProvider.ExecuteFunction(dcs, _appTaskParser);
        return records ?? [];
    }

    public async Task<List<AppTask>> GetTasksForAssignedToUser(int assignedToUserKey)
    {
        var dcs = GetFunctionCallDcsInstance("get_assigned_tasks_by_user_key");
        dcs.AddParameter("p_assigned_to_user_key", assignedToUserKey);

        var records = await NpgsqlDataProvider.ExecuteFunction(dcs, _appTaskParser);
        return records ?? [];
    }

    public async Task<List<AppTask>> GetTasksForCase(int caseKey)
    {
        var dcs = GetFunctionCallDcsInstance("get_tasks_by_case_key");
        dcs.AddParameter("p_case_key", caseKey);

        var records = await NpgsqlDataProvider.ExecuteFunction(dcs, _appTaskParser);
        return records ?? [];
    }

    public async Task<AppTask> GetTask(int taskKey)
    {
        var dcs = GetFunctionCallDcsInstance("get_task_by_task_key");
        AddTaskKeyParameter(dcs, taskKey);

        var records = await NpgsqlDataProvider.ExecuteFunction(dcs, _appTaskParser);
        return records.FirstOrDefault() ?? new();
    }

    public async Task<int> AddTask(AppTask task, int insertedByUserKey)
    {
        var dcs = GetFunctionCallDcsInstance("add_task");
        AddTaskParameters(task, dcs);
        dcs.AddParameter("p_inserted_by_user_key", insertedByUserKey);

        var taskKey = await NpgsqlDataProvider.ExecuteFunctionScalar(dcs, _intParser);
        return taskKey;
    }

    public async Task<int> UpdateTask(AppTask task, int updatedByUserKey)
    {
        var dcs = GetFunctionCallDcsInstance("update_task");
        AddTaskKeyParameter(dcs, task.TaskKey);
        AddTaskParameters(task, dcs);
        dcs.AddParameter("p_updated_by_user_key", updatedByUserKey);

        var taskKey = await NpgsqlDataProvider.ExecuteFunctionScalar(dcs, _intParser);
        return taskKey;
    }

    public Task<int> DeleteTask(int taskKey, int deletedByUserKey)
    {
        var dcs = GetFunctionCallDcsInstance("delete_task");
        AddTaskKeyParameter(dcs, taskKey);

        var deletedTaskKey = NpgsqlDataProvider.ExecuteFunctionScalar(dcs, _intParser);
        return deletedTaskKey;
    }

    private static void AddTaskKeyParameter(DataCallSettings dcs, int taskKey)
    {
        dcs.AddParameter("p_task_key", taskKey);
    }

    private static void AddTaskParameters(AppTask task, DataCallSettings dcs)
    {
        dcs.AddParameter("p_task_name", task.TaskName);
        dcs.AddParameter("p_task_description", task.TaskDescription);
        dcs.AddParameter("p_case_key", task.CaseKey);
        dcs.AddParameter("p_assigned_to_user_key", task.AssignedToUserKey);
        dcs.AddParameter("p_reminder_date", task.ReminderDate);
        dcs.AddParameter("p_due_date", task.DueDate);
    }
}
