namespace LunaEdge.TestAssignment.Application.Features.Tasks.Dtos;

public class TaskSortDto
{
    public string SortBy { get; set; } = "dueDate";
    public bool IsAscending { get; set; } = true; 
}