using LunaEdge.TestAssignment.Domain.Entities;

namespace LunaEdge.TestAssignment.Application.Features.Jwt;

public interface IJwtTokenService
{
    string GenerateJwtToken(User user);
}