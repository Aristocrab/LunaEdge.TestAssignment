using Isopoh.Cryptography.Argon2;

namespace Service.Users.PasswordHashing;

public class PasswordHashingService : IPasswordHashingService
{
    public string HashPassword(string password)
    {
        return Argon2.Hash(password);
    }

    public bool Verify(string password, string hash)
    {
        return Argon2.Verify(hash, password);
    }
}