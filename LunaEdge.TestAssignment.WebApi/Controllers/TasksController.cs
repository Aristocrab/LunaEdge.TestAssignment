using LunaEdge.TestAssignment.Application.Features.Tasks;
using LunaEdge.TestAssignment.Application.Features.Tasks.Dtos;
using LunaEdge.TestAssignment.WebApi.Controllers.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LunaEdge.TestAssignment.WebApi.Controllers;

[Authorize]
public class TasksController : BaseController
{
    private readonly ITasksService _tasksService;

    public TasksController(ITasksService tasksService)
    {
        _tasksService = tasksService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetTasks()
    {
        var tasks = await _tasksService.GetTasks(CurrentUserId);
        return Ok(tasks);
    }
    
    [HttpGet("{taskId}")]
    public async Task<IActionResult> GetTask(Guid taskId)
    {
        var task = await _tasksService.GetTask(CurrentUserId, taskId);
        return Ok(task);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateTask(CreateTaskDto task)
    {
        var createdTask = await _tasksService.CreateTask(CurrentUserId, task);
        return Ok(createdTask);
    }
    
    [HttpPut("{taskId}")]
    public async Task<IActionResult> UpdateTask(Guid taskId, CreateTaskDto task)
    {
        var updatedTask = await _tasksService.UpdateTask(CurrentUserId, taskId, task);
        return Ok(updatedTask);
    }
    
    [HttpDelete("{taskId}")]
    public async Task<IActionResult> DeleteTask(Guid taskId)
    {
        await _tasksService.DeleteTask(CurrentUserId, taskId);
        return Ok();
    }
}