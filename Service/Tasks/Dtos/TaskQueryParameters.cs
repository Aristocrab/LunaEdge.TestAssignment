namespace Service.Tasks.Dtos;

public class TaskQueryParameters
{
    // Filtering
    public string? Status { get; set; }
    public DateTime? DueDate { get; set; }
    public string? Priority { get; set; }

    // Sorting
    public string? SortBy { get; set; } // "DueDate" or "Priority"
    public bool SortDescending { get; set; } = false;

    // Pagination
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}