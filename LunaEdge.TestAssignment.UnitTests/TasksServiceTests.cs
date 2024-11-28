using Bogus;
using FluentAssertions;
using FluentValidation;
using LunaEdge.TestAssignment.Application.Database.Repositories;
using LunaEdge.TestAssignment.Application.Features.Tasks;
using LunaEdge.TestAssignment.Application.Features.Tasks.Dtos;
using LunaEdge.TestAssignment.Application.Features.Tasks.Specifications;
using LunaEdge.TestAssignment.Domain.Entities;
using LunaEdge.TestAssignment.Domain.Enums;
using LunaEdge.TestAssignment.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace LunaEdge.TestAssignment.UnitTests;

public class TasksServiceTests
{
    private readonly IRepository<TaskItem> _tasksRepository;
    private readonly IRepository<User> _usersRepository;
    private readonly TasksService _tasksService;
    private readonly Faker _faker;

    public TasksServiceTests()
    {
        _tasksRepository = Substitute.For<IRepository<TaskItem>>();
        _usersRepository = Substitute.For<IRepository<User>>();
        var logger = Substitute.For<ILogger<TasksService>>();
        var createTaskValidator = Substitute.For<IValidator<CreateTaskDto>>();
        _tasksService = new TasksService(_tasksRepository, _usersRepository, logger, createTaskValidator);
        _faker = new Faker();
    }
    
    [Fact]
    public async Task GetTasks_Should_Return_Task_List_For_User()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var tasks = new List<TaskDto>
        {
            new() { Id = Guid.NewGuid(), Title = "Task 1" },
            new() { Id = Guid.NewGuid(), Title = "Task 2" }
        };

        _tasksRepository.ListAsync(Arg.Any<TaskDtoWithQuerySpec>()).Returns(tasks);

        // Act
        var result = await _tasksService.GetTasks(userId, null, null, null);

        // Assert
        result.Should().HaveCount(2);
        result.Should().BeEquivalentTo(tasks);
    }
    
    [Fact]
    public async Task GetTask_Should_Return_Task_If_Found()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var taskId = Guid.NewGuid();
        var task = new TaskDto { Id = taskId, Title = "Task 1" };

        _tasksRepository.FirstOrDefaultAsync(Arg.Any<TaskDtoByUserIdSpec>()).Returns(task);

        // Act
        var result = await _tasksService.GetTask(userId, taskId);

        // Assert
        result.Should().BeEquivalentTo(task);
    }

    [Fact]
    public async Task GetTask_Should_Throw_TaskNotFoundException_If_Task_Not_Found()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var taskId = Guid.NewGuid();
        _tasksRepository.FirstOrDefaultAsync(Arg.Any<TaskDtoByUserIdSpec>()).Returns((TaskDto?)null);

        // Act
        var action = async () => await _tasksService.GetTask(userId, taskId);

        // Assert
        await action.Should().ThrowAsync<TaskNotFoundException>();
    }

    [Fact]
    public async Task CreateTask_Should_Create_Task_If_Valid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var taskId = Guid.NewGuid();
        var user = new User { 
            Id = userId, 
            Email = _faker.Person.Email,
            Username = _faker.Person.UserName,
            PasswordHash = _faker.Random.Hash()
        };
        var createTaskDto = new CreateTaskDto
        {
            Title = "New Task",
            Description = "Task Description",
            DueDate = DateTime.UtcNow.AddDays(1),
            Status = Status.Pending,
            Priority = Priority.High
        };

        var newTask = new TaskItem { Id = taskId, Title = createTaskDto.Title, User = user };

        _usersRepository.GetByIdAsync(userId).Returns(user);
        _tasksRepository.AddAsync(Arg.Any<TaskItem>()).Returns(newTask);

        // Act
        var result = await _tasksService.CreateTask(userId, createTaskDto);

        // Assert
        result.Should().Be(taskId);
        await _tasksRepository.Received(1).AddAsync(Arg.Any<TaskItem>());
    }

    [Fact]
    public async Task CreateTask_Should_Throw_UserNotFoundException_If_User_Not_Found()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var createTaskDto = new CreateTaskDto { Title = "New Task" };

        _usersRepository.GetByIdAsync(userId).Returns((User?)null);

        // Act
        var action = async () => await _tasksService.CreateTask(userId, createTaskDto);

        // Assert
        await action.Should().ThrowAsync<UserNotFoundException>();
        await _tasksRepository.DidNotReceive().AddAsync(Arg.Any<TaskItem>());
    }


    [Fact]
    public async Task UpdateTask_Should_Update_Task_If_Found()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { 
            Id = userId, 
            Email = _faker.Person.Email,
            Username = _faker.Person.UserName,
            PasswordHash = _faker.Random.Hash()
        };
        var taskId = Guid.NewGuid();
        var existingTask = new TaskItem { Id = taskId, Title = "Old Title", User = user };
        var updateTaskDto = new CreateTaskDto { Title = "New Title" };

        _tasksRepository.FirstOrDefaultAsync(Arg.Any<TaskByUserIdSpec>()).Returns(existingTask);

        // Act
        var result = await _tasksService.UpdateTask(userId, taskId, updateTaskDto);

        // Assert
        result.Should().Be(taskId);
        existingTask.Title.Should().Be("New Title");
        await _tasksRepository.Received(1).UpdateAsync(existingTask);
    }

    [Fact]
    public async Task DeleteTask_Should_Delete_Task_If_Found()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { 
            Id = userId, 
            Email = _faker.Person.Email,
            Username = _faker.Person.UserName,
            PasswordHash = _faker.Random.Hash()
        };
        var taskId = Guid.NewGuid();
        var task = new TaskItem { Id = taskId, Title = "Task 1", User = user };

        _tasksRepository.FirstOrDefaultAsync(Arg.Any<TaskByUserIdSpec>()).Returns(task);

        // Act
        await _tasksService.DeleteTask(userId, taskId);

        // Assert
        await _tasksRepository.Received(1).DeleteAsync(task);
    }

    [Fact]
    public async Task DeleteTask_Should_Not_Throw_If_Task_Not_Found()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var taskId = Guid.NewGuid();

        _tasksRepository.FirstOrDefaultAsync(Arg.Any<TaskByUserIdSpec>()).Returns((TaskItem?)null);

        // Act
        var action = async () => await _tasksService.DeleteTask(userId, taskId);

        // Assert
        await action.Should().NotThrowAsync();
    }
}