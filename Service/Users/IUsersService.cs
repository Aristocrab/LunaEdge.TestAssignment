using ErrorOr;
using Service.Users.Dtos;

namespace Service.Users;

public interface IUsersService
{
    Task<ErrorOr<Created>> Register(RegisterDto registerDto);
    Task<ErrorOr<string>> Login(LoginDto loginDto);
}