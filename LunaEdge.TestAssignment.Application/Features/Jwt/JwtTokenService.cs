using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LunaEdge.TestAssignment.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LunaEdge.TestAssignment.Application.Features.Jwt;

public class JwtTokenService : IJwtTokenService
{
    private readonly string _secretKey;
    private readonly string _issuer;

    public JwtTokenService(IConfiguration configuration)
    {
        _secretKey = configuration["Jwt:SecretKey"]!; 
        _issuer = configuration["Jwt:Issuer"]!;
    }

    public string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim("userId", user.Id.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _issuer,
            claims: claims,
            expires: DateTime.Now.AddDays(14),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
