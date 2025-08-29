using Domain.Entities.Shared;
using Domain.Enums;

namespace Domain.Entities;

public class TodoTask : BaseEntity
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public TodoTaskStatus Status { get; set; }
    public TodoTaskPriority Priority { get; set; }
    
    public User User { get; set; } = null!;
}