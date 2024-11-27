namespace LunaEdge.TestAssignment.Domain.Exceptions;

public class TaskNotFoundException : Exception
{
    public TaskNotFoundException(Guid taskId) : base($"Task with id {taskId} not found") { }
}