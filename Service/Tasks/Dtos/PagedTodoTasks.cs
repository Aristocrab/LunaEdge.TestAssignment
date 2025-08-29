namespace Service.Tasks.Dtos;

public class PagedTodoTasks
{
    public List<TodoTaskDto> Items { get; set; } = [];
    public required int TotalCount { get; set; }
    public required int Page { get; set; }
    public required int PageSize { get; set; }
}