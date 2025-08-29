using Domain.Entities;
using ErrorOr;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Service.Users.Dtos;
using Service.Users.JwtTokens;
using Service.Users.PasswordHashing;
using Service.Users.Repositories;

namespace Service.Users;

public class UsersService : IUsersService
{
    private readonly IValidator<RegisterDto> _registerDtoValidator;
    private readonly IValidator<LoginDto> _loginDtoValidator;
    private readonly ILogger<UsersService> _logger;
    private readonly IUsersRepository _usersRepository;
    private readonly IPasswordHashingService _passwordHashingService;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public UsersService(IUsersRepository usersRepository, 
        IPasswordHashingService passwordHashingService, 
        IJwtTokenGenerator jwtTokenGenerator,
        IValidator<RegisterDto> registerDtoValidator,
        IValidator<LoginDto> loginDtoValidator,
        ILogger<UsersService> logger)
    {
        _usersRepository = usersRepository;
        _passwordHashingService = passwordHashingService;
        _jwtTokenGenerator = jwtTokenGenerator;
        _registerDtoValidator = registerDtoValidator;
        _loginDtoValidator = loginDtoValidator;
        _logger = logger;
    }
    
    public async Task<ErrorOr<string>> Register(RegisterDto registerDto)
    {
        var validationResult = await _registerDtoValidator.ValidateAsync(registerDto);
        if (!validationResult.IsValid)
        {
            return Error.Validation(description: validationResult.Errors.First().ErrorMessage);
        }

        if (await _usersRepository.ExistsByUsernameAsync(registerDto.Username))
        {
            return Error.Conflict(description: "Username is already taken");
        }

        if (await _usersRepository.ExistsByEmailAsync(registerDto.Username))
        {
            return Error.Conflict(description: "Email is already registered");
        }

        var newUser = new User
        {
            Username = registerDto.Username,
            Email = registerDto.Email,
            PasswordHash = _passwordHashingService.HashPassword(registerDto.Password),
        };

        await _usersRepository.AddAsync(newUser);
        
        _logger.LogInformation("User {Username} has been registered", newUser.Username);

        return _jwtTokenGenerator.Generate(newUser);
    }

    public async Task<ErrorOr<string>> Login(LoginDto loginDto)
    {
        var validationResult = await _loginDtoValidator.ValidateAsync(loginDto);
        if (!validationResult.IsValid)
        {
            return Error.Validation(description: validationResult.Errors.First().ErrorMessage);
        }

        var user = await _usersRepository.GetByUsernameOrEmailAsync(loginDto.UsernameOrEmail);
        if (user is null)
        {
            return Error.NotFound(description: "User was not found");
        }

        if (!_passwordHashingService.Verify(loginDto.Password, user.PasswordHash))
        {
            return Error.Failure(description: "Password is incorrect");
        }
        
        _logger.LogInformation("User {Username} has been logged", user.Username);

        return _jwtTokenGenerator.Generate(user);
    }
}