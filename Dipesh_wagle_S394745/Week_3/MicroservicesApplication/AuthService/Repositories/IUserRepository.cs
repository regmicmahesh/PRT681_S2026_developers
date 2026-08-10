using AuthService.Models;

namespace AuthService.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserByUsernameAsync(string username);
    Task<int> CreateUserAsync(User user);
}
