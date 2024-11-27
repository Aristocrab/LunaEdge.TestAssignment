using LunaEdge.TestAssignment.Domain.Enums;

namespace LunaEdge.TestAssignment.Application.Features.Tasks.Dtos;

public class CreateTaskDto
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public Status Status { get; set; }
    public Priority Priority { get; set; }
}