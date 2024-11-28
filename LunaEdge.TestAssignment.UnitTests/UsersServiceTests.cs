using Bogus;
using FluentAssertions;
using FluentValidation;
using LunaEdge.TestAssignment.Application.Database.Repositories;
using LunaEdge.TestAssignment.Application.Features.Jwt;
using LunaEdge.TestAssignment.Application.Features.PasswordHashing;
using LunaEdge.TestAssignment.Application.Features.Users;
using LunaEdge.TestAssignment.Application.Features.Users.Dtos;
using LunaEdge.TestAssignment.Application.Features.Users.Specifications;
using LunaEdge.TestAssignment.Domain.Entities;
using LunaEdge.TestAssignment.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace LunaEdge.TestAssignment.UnitTests;

public class UsersServiceTests
{
    private readonly IRepository<User> _usersRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IPasswordHashingService _passwordHashingService;
    private readonly UsersService _usersService;
    private readonly Faker _faker;

    public UsersServiceTests()
    {
        _usersRepository = Substitute.For<IRepository<User>>();
        _jwtTokenService = Substitute.For<IJwtTokenService>();
        _passwordHashingService = Substitute.For<IPasswordHashingService>();
        var logger = Substitute.For<ILogger<UsersService>>();
        var validator = Substitute.For<IValidator<RegisterDto>>();
        _usersService = new UsersService(
            _usersRepository,
            _jwtTokenService, 
            _passwordHashingService,
            logger,
            validator);
        _faker = new Faker();
    }

    [Fact]
    public async Task RegisterUser_Should_Validate_Input()
    {
        // Arrange
        var registerDto = new RegisterDto
        {
            Username = _faker.Internet.UserName(),
            Email = _faker.Internet.Email(),
            Password = _faker.Internet.Password()
        };

        // Act
        var action = async () => await _usersService.RegisterUser(registerDto);

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact]
    public async Task RegisterUser_Should_Throw_When_Username_Already_Exists()
    {
        // Arrange
        var registerDto = new RegisterDto
        {
            Username = _faker.Internet.UserName(),
            Email = _faker.Internet.Email(),
            Password = _faker.Internet.Password()
        };

        _usersRepository.AnyAsync(Arg.Any<UserByUsernameSpec>()).Returns(true);

        // Act
        var action = async () => await _usersService.RegisterUser(registerDto);

        // Assert
        await action.Should().ThrowAsync<UserAlreadyExistsException>();
        await _usersRepository.Received(1).AnyAsync(Arg.Any<UserByUsernameSpec>());
    }

    [Fact]
    public async Task RegisterUser_Should_Add_User_When_Valid()
    {
        // Arrange
        var registerDto = new RegisterDto
        {
            Username = _faker.Internet.UserName(),
            Email = _faker.Internet.Email(),
            Password = _faker.Internet.Password()
        };

        _usersRepository.AnyAsync(Arg.Any<UserByUsernameSpec>()).Returns(false);
        _usersRepository.AnyAsync(Arg.Any<UserByEmailSpec>()).Returns(false);
        _passwordHashingService.HashPassword(registerDto.Password).Returns(_faker.Random.Hash());

        // Act
        await _usersService.RegisterUser(registerDto);

        // Assert
        await _usersRepository.Received(1).AddAsync(Arg.Is<User>(u => 
            u.Username == registerDto.Username &&
            u.Email == registerDto.Email &&
            !string.IsNullOrWhiteSpace(u.PasswordHash)));
    }

    [Fact]
    public async Task LoginUser_Should_Throw_When_User_Not_Found()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            UsernameOrEmail = _faker.Internet.Email(),
            Password = _faker.Internet.Password()
        };

        _usersRepository.FirstOrDefaultAsync(Arg.Any<UserByUsernameOrEmail>())
            .Returns((User?)null);

        // Act
        var action = async () => await _usersService.LoginUser(loginDto);

        // Assert
        await action.Should().ThrowAsync<UserNotFoundException>();
        await _usersRepository.Received(1).FirstOrDefaultAsync(Arg.Any<UserByUsernameOrEmail>());
    }

    [Fact]
    public async Task LoginUser_Should_Throw_When_Password_Invalid()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            UsernameOrEmail = _faker.Internet.Email(),
            Password = _faker.Internet.Password()
        };

        var user = new User
        {
            Username = _faker.Internet.UserName(),
            Email = loginDto.UsernameOrEmail,
            PasswordHash = _faker.Random.Hash()
        };

        _usersRepository.FirstOrDefaultAsync(Arg.Any<UserByUsernameOrEmail>()).Returns(user);
        _passwordHashingService.VerifyPassword(loginDto.Password, user.PasswordHash).Returns(false);

        // Act
        var action = async () => await _usersService.LoginUser(loginDto);

        // Assert
        await action.Should().ThrowAsync<InvalidPasswordException>();
        _passwordHashingService.Received(1).VerifyPassword(loginDto.Password, user.PasswordHash);
    }

    [Fact]
    public async Task LoginUser_Should_Return_Token_When_Valid()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            UsernameOrEmail = _faker.Internet.Email(),
            Password = _faker.Internet.Password()
        };

        var user = new User
        {
            Username = _faker.Internet.UserName(),
            Email = loginDto.UsernameOrEmail,
            PasswordHash = _faker.Random.Hash()
        };

        var token = _faker.Random.String2(32);

        _usersRepository.FirstOrDefaultAsync(Arg.Any<UserByUsernameOrEmail>()).Returns(user);
        _passwordHashingService.VerifyPassword(loginDto.Password, user.PasswordHash).Returns(true);
        _jwtTokenService.GenerateJwtToken(user).Returns(token);

        // Act
        var result = await _usersService.LoginUser(loginDto);

        // Assert
        result.Should().Be(token);
        _jwtTokenService.Received(1).GenerateJwtToken(user);
    }
}