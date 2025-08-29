using Domain.Entities;

namespace Service.Users.Repositories;

public interface IUsersRepository
{
    Task<List<User>> GetAllAsync();
    Task<User?> GetByIdAsync(Guid userId);
    Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail);
    Task AddAsync(User user);
    Task<bool> ExistsByUsernameAsync(string username);
    Task<bool> ExistsByEmailAsync(string email);
    Task SaveChangesAsync();
}