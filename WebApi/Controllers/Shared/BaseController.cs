using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Shared;

[ApiController]
[Route("[controller]")]
public abstract class BaseController : ControllerBase
{
    protected Guid UserId
    {
        get
        {
            if (HttpContext.User.Identity is not ClaimsIdentity identity)
            {
                return Guid.Empty;
            }

            var userClaims = identity.Claims.ToArray();
            if (userClaims.Length == 0)
            {
                return Guid.Empty;
            }

            return Guid.Parse(userClaims.First(x => x.Type == "userId").Value);
        }
    }

    protected ObjectResult UnauthorizedResult => Problem(
        title: "Unauthorized",
        detail: "Log in to perform this action",
        statusCode: StatusCodes.Status401Unauthorized
    );
}