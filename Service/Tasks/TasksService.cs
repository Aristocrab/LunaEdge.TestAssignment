using Domain.Entities;
using ErrorOr;
using FluentValidation;
using Mapster;
using Microsoft.Extensions.Logging;
using Service.Tasks.Dtos;
using Service.Tasks.Repositories;

namespace Service.Tasks;

public class TasksService : ITasksService
{
    private readonly ITasksRepository _tasksRepository;
    private readonly IValidator<CreateTaskDto> _createTaskDtoValidator;
    private readonly IValidator<UpdateTaskDto> _updateTaskDtoValidator;
    private readonly ILogger<TasksService> _logger;

    public TasksService(ITasksRepository tasksRepository, 
        IValidator<CreateTaskDto> createTaskDtoValidator,
        IValidator<UpdateTaskDto> updateTaskDtoValidator,
        ILogger<TasksService> logger)
    {
        _tasksRepository = tasksRepository;
        _createTaskDtoValidator = createTaskDtoValidator;
        _updateTaskDtoValidator = updateTaskDtoValidator;
        _logger = logger;
    }
    
    public async Task<PagedTodoTasks> GetAllTasks(Guid userId, TaskQueryParameters queryParams)
    {
        var items = await _tasksRepository.GetTasksAsync(userId, queryParams);
        var totalCount = await _tasksRepository.CountTasksAsync(userId, queryParams);

        return new PagedTodoTasks
        {
            Items = items.Adapt<List<TodoTaskDto>>(),
            TotalCount = totalCount,
            Page = queryParams.Page,
            PageSize = queryParams.PageSize
        };
    }
    
    public async Task<ErrorOr<TodoTaskDto>> GetTaskById(Guid taskId, Guid userId)
    {
        var result = await _tasksRepository.GetByIdAsync(taskId, userId);
        if (result is null) return Error.NotFound(description: "Task was not found");

        return result.Adapt<TodoTaskDto>();
    }

    public async Task<ErrorOr<Created>> CreateTask(CreateTaskDto createTaskDto, Guid userId)
    {
        var validationResult = await _createTaskDtoValidator.ValidateAsync(createTaskDto);
        if (!validationResult.IsValid) 
            return Error.Validation(description: validationResult.Errors.First().ErrorMessage);

        var newTask = new TodoTask
        {
            Title = createTaskDto.Title,
            Description = createTaskDto.Description,
            DueDate = createTaskDto.DueDate,
        };
        
        await _tasksRepository.AddAsync(newTask, userId);
        
        _logger.LogInformation("Task {Title} has been created", newTask.Title);

        return Result.Created;
    }

    public async Task<ErrorOr<Updated>> UpdateTask(Guid taskId, UpdateTaskDto updateTaskDto, Guid userId)
    {
        var validationResult = await _updateTaskDtoValidator.ValidateAsync(updateTaskDto);
        if (!validationResult.IsValid) 
            return Error.Validation(description: validationResult.Errors.First().ErrorMessage);
        
        var existingTask = await _tasksRepository.GetByIdAsync(taskId, userId);
        if (existingTask is null)
            return Error.NotFound("Task was not found");

        existingTask.Title = updateTaskDto.Title;
        existingTask.Description = updateTaskDto.Description;
        existingTask.DueDate = updateTaskDto.DueDate;
        existingTask.Priority = updateTaskDto.Priority;
        existingTask.Status = updateTaskDto.Status;

        await _tasksRepository.SaveChangesAsync();
        
        _logger.LogInformation("Task {Title} has been updated", existingTask.Title);

        return Result.Updated;
    }

    public async Task<ErrorOr<Deleted>> DeleteTask(Guid taskId, Guid userId)
    {
        var existingTask = await _tasksRepository.GetByIdAsync(taskId, userId);
        if (existingTask is null)
            return Error.NotFound(description: "Task was not found");

        await _tasksRepository.DeleteAsync(existingTask);
        
        _logger.LogInformation("Task {Title} has been deleted", existingTask.Title);

        return Result.Deleted;
    }
}