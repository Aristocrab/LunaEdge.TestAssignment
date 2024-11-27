using LunaEdge.TestAssignment.Domain.Entities.Shared;

namespace LunaEdge.TestAssignment.Domain.Entities;

public class User : Entity
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }

    public List<TaskItem> Tasks { get; set; } = [];
}