namespace LunaEdge.TestAssignment.Application.Features.Users.Dtos;

public class LoginDto
{
    public required string UsernameOrEmail { get; set; }
    public required string Password { get; set; }
}