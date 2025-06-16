using UserAccountService.Models;

namespace UserAccountService.Services;

/// <summary>
/// Defines the contract for user management, including registration, authentication, retrieval, and searching.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Registers a new user with the specified details.
    /// </summary>
    /// <param name="name">The name of the user.</param>
    /// <param name="tag">The unique tag associated with the user.</param>
    /// <param name="email">The email address of the user.</param>
    /// <param name="password">The password for the user account.</param>
    /// <returns>A <see cref="User"/> object representing the registered user, or null if registration fails.</returns>
    Task<User?> RegisterUserAsync(string name, string tag, string email, string password);

    /// <summary>
    /// Authenticates a user using their tag or email and password.
    /// </summary>
    /// <param name="tagOrEmail">The tag or email of the user attempting to authenticate.</param>
    /// <param name="password">The password for the user account.</param>
    /// <returns>A <see cref="User"/> object representing the authenticated user, or null if authentication fails.</returns>
    Task<User?> AuthenticateUserAsync(string tagOrEmail, string password);

    /// <summary>
    /// Retrieves a user by their unique identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A <see cref="User"/> object representing the user, or null if the user is not found.</returns>
    Task<User?> GetUserByIdAsync(Guid userId);

    /// <summary>
    /// Searches for users based on a query string.
    /// </summary>
    /// <param name="query">The search query used to find users.</param>
    /// <returns>A collection of <see cref="User"/> objects matching the search criteria.</returns>
    Task<IEnumerable<User>> SearchUsersAsync(string query);
}