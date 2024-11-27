using LunaEdge.TestAssignment.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace LunaEdge.TestAssignment.WebApi.Controllers.Shared;

[ApiController]
[Route("[controller]")]
public abstract class BaseController : ControllerBase
{
    protected Guid CurrentUserId
    {
        get
        {
            var userId = User.FindFirst("userId")?.Value;
            if (userId is null) { throw new UserNotFoundException(); }
            
            return Guid.Parse(userId);
        }
    }
}