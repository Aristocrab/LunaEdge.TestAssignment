using Domain.Entities;
using ErrorOr;
using Service.Tasks.Dtos;

namespace Service.Tasks;

public interface ITasksService
{
    Task<PagedTodoTasks> GetAllTasks(Guid userId, TaskQueryParameters queryParams);
    Task<ErrorOr<TodoTask>> GetTaskById(Guid taskId, Guid userId);
    Task<ErrorOr<Created>> CreateTask(CreateTaskDto createTaskDto, Guid userId);
    Task<ErrorOr<Updated>> UpdateTask(Guid taskId, UpdateTaskDto updateTaskDto, Guid userId);
    Task<ErrorOr<Deleted>> DeleteTask(Guid taskId, Guid userId);
}