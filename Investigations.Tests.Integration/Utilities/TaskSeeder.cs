using Bogus;
using Investigations.Models;
using Npgsql;

namespace Investigations.Tests.Integration.Utilities;

public static class TaskSeeder
{
    public async static Task<List<AppTask>> SeedTasks(TestFixture fixture, int numberOfTasks)
    {
        var tasks = new List<AppTask>();
        for (int i = 0; i < numberOfTasks; i++)
        {
            var task = await SeedSingleTask(fixture);
            tasks.Add(task);
        }
        return tasks;
    }

    public async static Task<AppTask> SeedSingleTask(TestFixture fixture)
    {
        var seedTask = new Faker<AppTask>()
            .RuleFor(t => t.TaskName, f => f.Lorem.Sentence(3))
            .RuleFor(t => t.TaskDescription, f => f.Lorem.Paragraph())
            .RuleFor(t => t.DueDate, f => f.Date.Future())
            .RuleFor(t => t.IsCompleted, f => false)
            .Generate();

        await using (var connection = new NpgsqlConnection(fixture.ConnectionString))
        {
            connection.Open();

            await using (var cmd = new NpgsqlCommand("INSERT INTO tasks (task_name, task_description, due_date, is_completed, inserted_datetime, inserted_by_user_key) " +
                        "VALUES (@task_name, @task_description, @due_date, @is_completed, CURRENT_TIMESTAMP, 100) " +
                        "RETURNING task_key;", connection))
            {
                cmd.Parameters.AddWithValue("task_name", seedTask.TaskName);
                cmd.Parameters.AddWithValue("task_description", seedTask.TaskDescription);
                cmd.Parameters.AddWithValue("due_date", seedTask.DueDate);
                cmd.Parameters.AddWithValue("is_completed", seedTask.IsCompleted);

                var taskKey = (int)await cmd.ExecuteScalarAsync();
                seedTask.TaskKey = taskKey;
            }
        }

        return seedTask;
    }
}
