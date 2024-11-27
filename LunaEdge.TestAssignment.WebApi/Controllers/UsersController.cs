using LunaEdge.TestAssignment.Application.Features.Users;
using LunaEdge.TestAssignment.Application.Features.Users.Dtos;
using LunaEdge.TestAssignment.WebApi.Controllers.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LunaEdge.TestAssignment.WebApi.Controllers;

[AllowAnonymous]
public class UsersController : BaseController
{
    private readonly IUsersService _usersService;

    public UsersController(IUsersService usersService)
    {
        _usersService = usersService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(RegisterDto user)
    {
        await _usersService.RegisterUser(user);
        return Ok();
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> LoginUser(LoginDto user)
    {
        var token = await _usersService.LoginUser(user);
        return Ok(token);
    }
}