using Ardalis.Specification;
using LunaEdge.TestAssignment.Application.Features.Tasks.Dtos;
using LunaEdge.TestAssignment.Domain.Entities;

namespace LunaEdge.TestAssignment.Application.Features.Tasks.Specifications;

public sealed class TaskDtoByUserIdSpec : Specification<TaskItem, TaskDto>
{
    public TaskDtoByUserIdSpec(Guid userId)
    {
        Query
            .Select(x => new TaskDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                DueDate = x.DueDate,
                Status = x.Status,
                Priority = x.Priority
            })
            .Where(x => x.User.Id == userId);
    }
    
    public TaskDtoByUserIdSpec(Guid userId, Guid taskId)
    {
        Query
            .Select(x => new TaskDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                DueDate = x.DueDate,
                Status = x.Status,
                Priority = x.Priority
            })
            .Where(x => x.User.Id == userId && x.Id == taskId);
    }
}