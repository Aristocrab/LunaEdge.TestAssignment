namespace LunaEdge.TestAssignment.Application.Features.Users.Dtos;

public class RegisterDto
{
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}