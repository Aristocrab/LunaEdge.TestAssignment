using Ardalis.Specification;
using LunaEdge.TestAssignment.Domain.Entities;

namespace LunaEdge.TestAssignment.Application.Features.Tasks.Specifications;

public sealed class TaskByUserIdSpec : Specification<TaskItem>
{
    public TaskByUserIdSpec(Guid userId)
    {
        Query
            .Where(x => x.User.Id == userId);
    }
    
    public TaskByUserIdSpec(Guid userId, Guid taskId)
    {
        Query
            .Where(x => x.User.Id == userId && x.Id == taskId);
    }
}