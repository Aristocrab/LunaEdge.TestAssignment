namespace LunaEdge.TestAssignment.Application.Features.PasswordHashing;

public interface IPasswordHashingService
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword);
}