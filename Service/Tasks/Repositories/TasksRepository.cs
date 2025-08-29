using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Service.Database;
using Service.Tasks.Dtos;

namespace Service.Tasks.Repositories;

public class TasksRepository : ITasksRepository
{
    private readonly AppDbContext _context;

    public TasksRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<TodoTask>> GetTasksAsync(Guid userId, TaskQueryParameters queryParams)
    {
        var query = _context.TodoTasks.Where(x => x.User.Id == userId);

        // Filtering
        if (!string.IsNullOrEmpty(queryParams.Status))
            query = query.Where(x => x.Status.ToString().Equals(queryParams.Status, 
                StringComparison.CurrentCultureIgnoreCase));

        if (queryParams.DueDate.HasValue)
            query = query.Where(x => x.DueDate == queryParams.DueDate.Value);

        if (!string.IsNullOrEmpty(queryParams.Priority))
            query = query.Where(x => x.Priority.ToString().Equals(queryParams.Priority, 
                StringComparison.CurrentCultureIgnoreCase));

        // Sorting
        query = queryParams.SortBy?.ToLower() switch
        {
            "duedate" => queryParams.SortDescending ? query.OrderByDescending(t => t.DueDate) : query.OrderBy(t => t.DueDate),
            "priority" => queryParams.SortDescending ? query.OrderByDescending(t => t.Priority) : query.OrderBy(t => t.Priority),
            _ => query.OrderBy(t => t.CreatedAt)
        };

        // Pagination
        var skip = (queryParams.Page - 1) * queryParams.PageSize;
        return await query.Skip(skip).Take(queryParams.PageSize).ToListAsync();
    }

    public async Task<int> CountTasksAsync(Guid userId, TaskQueryParameters queryParams)
    {
        var query = _context.TodoTasks.Where(x => x.User.Id == userId);

        if (!string.IsNullOrEmpty(queryParams.Status))
            query = query.Where(x => x.Status.ToString().Equals(queryParams.Status, 
                StringComparison.CurrentCultureIgnoreCase));

        if (queryParams.DueDate.HasValue)
            query = query.Where(x => x.DueDate == queryParams.DueDate.Value);

        if (!string.IsNullOrEmpty(queryParams.Priority))
            query = query.Where(x => x.Priority.ToString().Equals(queryParams.Priority, 
                StringComparison.CurrentCultureIgnoreCase));
        
        return await query.CountAsync();
    }
    
    public async Task<TodoTask?> GetByIdAsync(Guid taskId, Guid userId)
    {
        return await _context.TodoTasks
            .Include(x => x.User)
            .FirstOrDefaultAsync(t => t.Id == taskId && t.User.Id == userId);
    }

    public async Task AddAsync(TodoTask task, Guid userId)
    {
        task.User = await _context.Users.FirstAsync(x => x.Id == userId);
        
        await _context.TodoTasks.AddAsync(task);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(TodoTask task)
    {
        _context.TodoTasks.Remove(task);
        await _context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}