using Domain.Enums;

namespace Service.Tasks.Dtos;

public record CreateTaskDto(string Title, string? Description, DateTime? DueDate, TodoTaskStatus Status, TodoTaskPriority Priority);