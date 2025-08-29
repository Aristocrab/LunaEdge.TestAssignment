namespace Service.Users.PasswordHashing;

public interface IPasswordHashingService
{
    string HashPassword(string password);
    bool Verify(string password, string hash);
}