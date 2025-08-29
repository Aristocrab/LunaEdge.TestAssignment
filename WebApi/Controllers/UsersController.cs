using Microsoft.AspNetCore.Mvc;
using Service.Users;
using Service.Users.Dtos;
using WebApi.Controllers.Shared;
using WebApi.Extensions;

namespace WebApi.Controllers;

public class UsersController : BaseController
{
    private readonly IUsersService _usersService;

    public UsersController(IUsersService usersService)
    {
        _usersService = usersService;
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<string>> Register(RegisterDto registerDto)
    {
        var result = await _usersService.Register(registerDto);

        return result.ToActionResult();
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(LoginDto loginDto)
    {
        var result = await _usersService.Login(loginDto);

        return result.ToActionResult();
    }
}