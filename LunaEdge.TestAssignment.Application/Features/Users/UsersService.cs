using LunaEdge.TestAssignment.Application.Database.Repositories;
using LunaEdge.TestAssignment.Application.Features.Jwt;
using LunaEdge.TestAssignment.Application.Features.PasswordHashing;
using LunaEdge.TestAssignment.Application.Features.Users.Dtos;
using LunaEdge.TestAssignment.Application.Features.Users.Specifications;
using LunaEdge.TestAssignment.Domain.Entities;
using LunaEdge.TestAssignment.Domain.Exceptions;
using Throw;

namespace LunaEdge.TestAssignment.Application.Features.Users;

public class UsersService : IUsersService
{
    private readonly IRepository<User> _usersRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IPasswordHashingService _passwordHashingService;

    public UsersService(IRepository<User> usersRepository, 
        IJwtTokenService jwtTokenService, 
        IPasswordHashingService passwordHashingService)
    {
        _usersRepository = usersRepository;
        _jwtTokenService = jwtTokenService;
        _passwordHashingService = passwordHashingService;
    }

    public async Task RegisterUser(RegisterDto user)
    {
        var userByUsernameSpec = new UserByUsernameSpec(user.Username);
        var userWithSameUsernameExists = await _usersRepository.AnyAsync(userByUsernameSpec);
        userWithSameUsernameExists
            .Throw(_ => new UserAlreadyExistsException())
            .IfTrue();
        
        var userByEmailSpec = new UserByEmailSpec(user.Email);
        var userWithSameEmailExists = await _usersRepository.AnyAsync(userByEmailSpec);
        userWithSameEmailExists
            .Throw(_ => new UserAlreadyExistsException())
            .IfTrue();
        
        var newUser = new User
        {
            Username = user.Username,
            Email = user.Email,
            PasswordHash = _passwordHashingService.HashPassword(user.Password)
        };
        
        await _usersRepository.AddAsync(newUser);
    }

    public async Task<string> LoginUser(LoginDto loginDto)
    {
        var userByUsernameOrEmailSpec = new UserByUsernameOrEmail(loginDto.UsernameOrEmail);
        var user = await _usersRepository.FirstOrDefaultAsync(userByUsernameOrEmailSpec);
        user.ThrowIfNull(_ => new UserNotFoundException());
        
        var isPasswordCorrect = _passwordHashingService.VerifyPassword(loginDto.Password, user.PasswordHash);
        isPasswordCorrect
            .Throw(_ => new InvalidPasswordException())
            .IfFalse();
        
        return _jwtTokenService.GenerateJwtToken(user);
    }
}