namespace Investigations.Models.Repositories;

public interface IAppTaskRepository
{
    Task<List<AppTask>> GetTasks(int assignedToUserKey);
    Task<AppTask> GetTask(int taskKey);
    Task<List<AppTask>> GetTasksForAssignedToUser(int assignedToUserKey);
    Task<List<AppTask>> GetTasksForCase(int caseKey);
    Task<int> AddTask(AppTask task, int insertedByUserKey);
    Task<int> UpdateTask(AppTask task, int updatedByUserKey);
    Task<int> DeleteTask(int taskKey, int deletedByUserKey);
}
