using LunaEdge.TestAssignment.Application.Database.Repositories;
using LunaEdge.TestAssignment.Application.Features.Tasks.Dtos;
using LunaEdge.TestAssignment.Application.Features.Tasks.Specifications;
using LunaEdge.TestAssignment.Domain.Entities;
using LunaEdge.TestAssignment.Domain.Exceptions;
using Throw;

namespace LunaEdge.TestAssignment.Application.Features.Tasks;

public class TasksService : ITasksService
{
    private readonly IRepository<TaskItem> _tasksRepository;
    private readonly IRepository<User> _usersRepository;

    public TasksService(IRepository<TaskItem> tasksRepository, IRepository<User> usersRepository)
    {
        _tasksRepository = tasksRepository;
        _usersRepository = usersRepository;
    }
    
    public Task<List<TaskDto>> GetTasks(Guid userId)
    {
        var taskByUserId = new TaskDtoByUserIdSpec(userId);
        return _tasksRepository.ListAsync(taskByUserId);
    }

    public async Task<TaskDto> GetTask(Guid userId, Guid taskId)
    {
        var taskByUserId = new TaskDtoByUserIdSpec(userId, taskId);
        var task = await _tasksRepository.FirstOrDefaultAsync(taskByUserId);

        task.ThrowIfNull(_ => new TaskNotFoundException(taskId));
        
        return task;
    }

    public async Task<Guid> CreateTask(Guid userId, CreateTaskDto task)
    {
        var user = await _usersRepository.GetByIdAsync(userId);
        
        user.ThrowIfNull(_ => new UserNotFoundException(userId));
        
        var newTask = new TaskItem
        {
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate,
            User = user
        };
        
        var res = await _tasksRepository.AddAsync(newTask);
        
        return res.Id;
    }

    public async Task<Guid> UpdateTask(Guid userId, Guid taskId, CreateTaskDto task)
    {
        var taskByUserId = new TaskByUserIdSpec(userId, taskId);
        var existingTask = await _tasksRepository.FirstOrDefaultAsync(taskByUserId);
        
        existingTask.ThrowIfNull(_ => new TaskNotFoundException(taskId));
        
        existingTask.Title = task.Title;
        existingTask.Description = task.Description;
        existingTask.DueDate = task.DueDate;
        
        await _tasksRepository.UpdateAsync(existingTask);
        
        return existingTask.Id;
    }

    public async Task DeleteTask(Guid userId, Guid taskId)
    {
        var taskByUserId = new TaskByUserIdSpec(userId, taskId);
        var task = await _tasksRepository.FirstOrDefaultAsync(taskByUserId);
        if (task is null) return;
        
        await _tasksRepository.DeleteAsync(task);
    }
}