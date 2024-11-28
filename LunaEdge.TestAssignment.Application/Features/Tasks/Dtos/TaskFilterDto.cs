namespace LunaEdge.TestAssignment.Application.Features.Tasks.Dtos;

public class TaskFilterDto
{
    public int? Status { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public int? Priority { get; set; }
}