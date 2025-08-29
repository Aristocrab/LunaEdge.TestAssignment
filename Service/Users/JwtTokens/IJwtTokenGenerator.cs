using Domain.Entities;

namespace Service.Users.JwtTokens;

public interface IJwtTokenGenerator
{
    string Generate(User user);
}