namespace LunaEdge.TestAssignment.Domain.Exceptions;

public class InvalidPasswordException : Exception
{
    public InvalidPasswordException() : base("Invalid password") { }
}