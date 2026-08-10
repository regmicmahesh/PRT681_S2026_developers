using TaskService.Models;

namespace TaskService.Repositories;

public interface ITaskRepository
{
    Task<IEnumerable<TaskItem>> GetTasksByUserIdAsync(int userId);
    Task<TaskItem?> GetTaskByIdAsync(int id, int userId);
    Task<int> CreateTaskAsync(TaskItem task);
    Task<bool> UpdateTaskAsync(TaskItem task);
    Task<bool> DeleteTaskAsync(int id, int userId);
}
