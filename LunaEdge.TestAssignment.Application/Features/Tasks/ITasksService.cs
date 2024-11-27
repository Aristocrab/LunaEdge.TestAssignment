using LunaEdge.TestAssignment.Application.Features.Tasks.Dtos;
using LunaEdge.TestAssignment.Domain.Entities;

namespace LunaEdge.TestAssignment.Application.Features.Tasks;

public interface ITasksService
{
    Task<List<TaskItem>> GetTasks(Guid userId);
    Task<TaskItem> GetTask(Guid userId, Guid taskId);
    Task<TaskItem> CreateTask(Guid userId, CreateTaskDto task);
    Task<TaskItem> UpdateTask(Guid userId, Guid taskId, CreateTaskDto task);
    Task DeleteTask(Guid userId, Guid taskId);
}