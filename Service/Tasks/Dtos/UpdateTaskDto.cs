using Domain.Enums;

namespace Service.Tasks.Dtos;

public record UpdateTaskDto(string Title, string? Description, DateTime? DueDate, TodoTaskStatus Status, TodoTaskPriority Priority);