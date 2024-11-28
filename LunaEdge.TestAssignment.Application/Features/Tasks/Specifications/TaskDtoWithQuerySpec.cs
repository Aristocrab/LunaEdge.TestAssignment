using Ardalis.Specification;
using LunaEdge.TestAssignment.Application.Features.Tasks.Dtos;
using LunaEdge.TestAssignment.Domain.Entities;

namespace LunaEdge.TestAssignment.Application.Features.Tasks.Specifications;

public sealed class TaskDtoWithQuerySpec : Specification<TaskItem, TaskDto>
{
    public TaskDtoWithQuerySpec(Guid userId, TaskFilterDto? filter, TaskSortDto? sort, PaginationDto? pagination)
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

        // Apply filters
        if (filter is not null)
        {
            if (filter.Status.HasValue)
            {
                Query.Where(x => (int)x.Status == filter.Status);
            }

            if (filter.DateFrom.HasValue)
            {
                Query.Where(x => x.DueDate >= filter.DateFrom.Value);
            }
            
            if (filter.DateTo.HasValue)
            {
                Query.Where(x => x.DueDate <= filter.DateTo.Value);
            }

            if (filter.Priority.HasValue)
            {
                Query.Where(x => (int)x.Priority == filter.Priority.Value);
            }
        }

        // Apply sorting
        if (sort is not null)
        {
            if (sort.SortBy.Equals("dueDate", StringComparison.OrdinalIgnoreCase))
            {
                if (sort.IsAscending)
                    Query.OrderBy(x => x.DueDate);
                else
                    Query.OrderByDescending(x => x.DueDate);
            }
            else if (sort.SortBy.Equals("priority", StringComparison.OrdinalIgnoreCase))
            {
                if (sort.IsAscending)
                    Query.OrderBy(x => x.Priority);
                else
                    Query.OrderByDescending(x => x.Priority);
            }
        }
        else
        {
            // Default sort by DueDate ascending
            Query.OrderBy(x => x.DueDate);
        }

        // Apply pagination
        if (pagination is not null)
        {
            Query
                .Skip(pagination.PageSize * (pagination.Page - 1))
                .Take(pagination.PageSize);
        }
    }
}
