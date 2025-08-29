using System.Security.Claims;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Service.Tasks;
using Service.Tasks.Dtos;
using WebApi.Controllers.Shared;
using WebApi.Extensions;

namespace WebApi.Controllers;

public class TasksController : BaseController
{
    private readonly ITasksService _tasksService;

    public TasksController(ITasksService tasksService)
    {
        _tasksService = tasksService;
    }
    
    [HttpGet]
    public async Task<ActionResult<PagedTodoTasks>> GetAllTasks([FromQuery] TaskQueryParameters queryParams)
    {
        if (UserId == Guid.Empty) return UnauthorizedResult;
        
        return await _tasksService.GetAllTasks(UserId, queryParams);
    } 
    
    [HttpGet("{taskId:guid}")]
    public async Task<ActionResult<TodoTaskDto>> GetTaskById(Guid taskId)
    {
        if (UserId == Guid.Empty) return UnauthorizedResult;
        
        var result = await _tasksService.GetTaskById(taskId, UserId);

        return result.ToActionResult();
    } 
    
    [HttpPost]
    public async Task<ActionResult> CreateTask(CreateTaskDto createTaskDto)
    {
        if (UserId == Guid.Empty) return UnauthorizedResult;
        
        var result = await _tasksService.CreateTask(createTaskDto, UserId);

        return result.Match<ActionResult>(
            _ => Created(),
            errors => errors[0].ToProblemDetails());
    } 
    
    [HttpPut("{taskId:guid}")]
    public async Task<ActionResult> UpdateTask(Guid taskId, UpdateTaskDto updateTaskDto)
    {
        if (UserId == Guid.Empty) return UnauthorizedResult;
        
        var result = await _tasksService.UpdateTask(taskId, updateTaskDto, UserId);

        return result.Match<ActionResult>(
            _ => NoContent(),
            errors => errors[0].ToProblemDetails());
    }

    [HttpDelete("{taskId:guid}")]
    public async Task<ActionResult> DeleteTask(Guid taskId)
    {
        if (UserId == Guid.Empty) return UnauthorizedResult;
        
        var result = await _tasksService.DeleteTask(taskId, UserId);

        return result.Match<ActionResult>(
            _ => NoContent(),
            errors => errors[0].ToProblemDetails());
    }
}