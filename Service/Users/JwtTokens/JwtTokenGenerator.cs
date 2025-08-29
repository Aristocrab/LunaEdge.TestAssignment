using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Service.Users.JwtTokens;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IConfiguration _configuration;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public string Generate(User user)
    {
        var claims = new List<Claim>
        {
            new("username", user.Username),
            new("userId", user.Id.ToString()),
        };

        var secretBytes = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"] ?? "");
        var key = new SymmetricSecurityKey(secretBytes);
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            audience: _configuration["Jwt:Audience"],
            issuer: _configuration["Jwt:Issuer"],
            claims: claims,
            notBefore: DateTime.Now,
            signingCredentials: signingCredentials);

        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
        return jwtToken;
    }
}