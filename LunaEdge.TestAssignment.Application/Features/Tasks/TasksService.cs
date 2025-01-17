using FluentValidation;
using LunaEdge.TestAssignment.Application.Database.Repositories;
using LunaEdge.TestAssignment.Application.Features.Tasks.Dtos;
using LunaEdge.TestAssignment.Application.Features.Tasks.Specifications;
using LunaEdge.TestAssignment.Domain.Entities;
using LunaEdge.TestAssignment.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using Throw;

namespace LunaEdge.TestAssignment.Application.Features.Tasks;

public class TasksService : ITasksService
{
    private readonly IRepository<TaskItem> _tasksRepository;
    private readonly IRepository<User> _usersRepository;
    private readonly ILogger<TasksService> _logger;
    private readonly IValidator<CreateTaskDto> _createTaskDto;

    public TasksService(IRepository<TaskItem> tasksRepository, 
        IRepository<User> usersRepository,
        ILogger<TasksService> logger,
        IValidator<CreateTaskDto> createTaskDto)
    {
        _tasksRepository = tasksRepository;
        _usersRepository = usersRepository;
        _logger = logger;
        _createTaskDto = createTaskDto;
    }
    
    public Task<List<TaskDto>> GetTasks(Guid userId, 
        TaskFilterDto? filter, 
        TaskSortDto? sort, 
        PaginationDto? pagination)
    {
        var taskDtoWithQuerySpec = new TaskDtoWithQuerySpec(userId, filter, sort, pagination);
        return _tasksRepository.ListAsync(taskDtoWithQuerySpec);
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
        await _createTaskDto.ValidateAndThrowAsync(task);
        
        var user = await _usersRepository.GetByIdAsync(userId);
        
        user.ThrowIfNull(_ => new UserNotFoundException(userId));
        
        var newTask = new TaskItem
        {
            Title = task.Title,
            Description = task.Description,
            DueDate = task.DueDate,
            User = user,
            Status = task.Status,
            Priority = task.Priority
        };
        
        var res = await _tasksRepository.AddAsync(newTask);
        
        _logger.LogInformation("Created new task with title {Title} for user {UserId}", 
            newTask.Title, 
            user.Id);
        
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
        existingTask.Status = task.Status;
        existingTask.Priority = task.Priority;
        
        await _tasksRepository.UpdateAsync(existingTask);
        
        _logger.LogInformation("Updated task with id {TaskId} for user {UserId}", 
            existingTask.Id, 
            userId);
        
        return existingTask.Id;
    }

    public async Task DeleteTask(Guid userId, Guid taskId)
    {
        var taskByUserId = new TaskByUserIdSpec(userId, taskId);
        var task = await _tasksRepository.FirstOrDefaultAsync(taskByUserId);
        if (task is null) return;
        
        _logger.LogInformation("Deleted task with id {TaskId} for user {UserId}", 
            task.Id, 
            userId);
        
        await _tasksRepository.DeleteAsync(task);
    }
}