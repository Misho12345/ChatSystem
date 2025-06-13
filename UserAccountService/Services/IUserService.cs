using UserAccountService.Models;

namespace UserAccountService.Services;

public interface IUserService
{
    Task<User?> RegisterUserAsync(string name, string tag, string email, string password);
    Task<User?> AuthenticateUserAsync(string tagOrEmail, string password);
    Task<User?> GetUserByIdAsync(Guid userId);
    Task<IEnumerable<User>> SearchUsersAsync(string query);
}