namespace LunaEdge.TestAssignment.Domain.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException(Guid userId) : base($"User with id {userId} not found") { }

    public UserNotFoundException() : base("User not found") { }
}