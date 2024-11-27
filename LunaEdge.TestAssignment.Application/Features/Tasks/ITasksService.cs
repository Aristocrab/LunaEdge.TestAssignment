using LunaEdge.TestAssignment.Application.Features.Tasks.Dtos;

namespace LunaEdge.TestAssignment.Application.Features.Tasks;

public interface ITasksService
{
    Task<List<TaskDto>> GetTasks(Guid userId);
    Task<TaskDto> GetTask(Guid userId, Guid taskId);
    Task<Guid> CreateTask(Guid userId, CreateTaskDto task);
    Task<Guid> UpdateTask(Guid userId, Guid taskId, CreateTaskDto task);
    Task DeleteTask(Guid userId, Guid taskId);
}