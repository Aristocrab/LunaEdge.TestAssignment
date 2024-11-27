using LunaEdge.TestAssignment.Domain.Entities.Shared;
using LunaEdge.TestAssignment.Domain.Enums;

namespace LunaEdge.TestAssignment.Domain.Entities;

public class TaskItem : Entity
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public Status Status { get; set; }
    public Priority Priority { get; set; }

    public required User User { get; set; }
}