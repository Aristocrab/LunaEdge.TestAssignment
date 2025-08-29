using Domain.Entities;
using Service.Tasks.Dtos;

namespace Service.Tasks.Repositories;

public interface ITasksRepository
{
    Task<List<TodoTask>> GetTasksAsync(Guid userId, TaskQueryParameters queryParams);
    Task<int> CountTasksAsync(Guid userId, TaskQueryParameters queryParams);
    Task<TodoTask?> GetByIdAsync(Guid taskId, Guid userId);
    Task AddAsync(TodoTask task, Guid userId);
    Task DeleteAsync(TodoTask task);
    Task SaveChangesAsync();
}