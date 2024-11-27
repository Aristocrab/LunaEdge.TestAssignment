using LunaEdge.TestAssignment.Application.Features.Users.Dtos;

namespace LunaEdge.TestAssignment.Application.Features.Users;

public interface IUsersService
{
    Task RegisterUser(RegisterDto registerDto);
    Task<string> LoginUser(LoginDto loginDto);
}