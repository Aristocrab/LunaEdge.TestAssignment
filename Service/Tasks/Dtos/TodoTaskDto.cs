using Domain.Enums;

namespace Service.Tasks.Dtos;

public class TodoTaskDto
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public TodoTaskStatus Status { get; set; }
    public TodoTaskPriority Priority { get; set; }
}