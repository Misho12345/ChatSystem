using UserAccountService.Data;
using UserAccountService.Models;
using Microsoft.EntityFrameworkCore;

namespace UserAccountService.Services;

/// <summary>
/// Provides methods for user management, including registration, authentication, retrieval, and searching.
/// </summary>
public class UserService(UserAccountDbContext context, ILogger<UserService> logger) : IUserService
{
    /// <summary>
    /// Registers a new user with the specified details.
    /// </summary>
    /// <param name="name">The name of the user.</param>
    /// <param name="tag">The unique tag associated with the user.</param>
    /// <param name="email">The email address of the user.</param>
    /// <param name="password">The password for the user account.</param>
    /// <returns>A <see cref="User"/> object representing the registered user, or null if registration fails.</returns>
    public async Task<User?> RegisterUserAsync(string name, string tag, string email, string password)
    {
        if (await context.Users.AnyAsync(u => u.Tag == tag || u.Email == email))
        {
            logger.LogWarning("Registration attempt failed: Tag '{Tag}' or Email '{Email}' already exists.", tag,
                email);
            return null;
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = name,
            Tag = tag.Trim(),
            Email = email.Trim().ToLowerInvariant(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            CreatedAt = DateTime.UtcNow
        };

        try
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
            logger.LogInformation("User registered successfully: {UserId}, Tag: {Tag}", user.Id, user.Tag);
            return user;
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, "Error registering user with Tag '{Tag}'. Database update failed.", tag);
            return null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error registering user with Tag '{Tag}'.", tag);
            return null;
        }
    }

    /// <summary>
    /// Authenticates a user using their tag or email and password.
    /// </summary>
    /// <param name="tagOrEmail">The tag or email of the user attempting to authenticate.</param>
    /// <param name="password">The password for the user account.</param>
    /// <returns>A <see cref="User"/> object representing the authenticated user, or null if authentication fails.</returns>
    public async Task<User?> AuthenticateUserAsync(string tagOrEmail, string password)
    {
        var normalizedIdentifier = tagOrEmail.Trim().ToLowerInvariant();
        var user = await context.Users
            .FirstOrDefaultAsync(u =>
                u.Tag.ToLower() == normalizedIdentifier || u.Email.ToLower() == normalizedIdentifier);

        if (user == null)
        {
            logger.LogWarning("Authentication failed: User not found for identifier '{Identifier}'.", tagOrEmail);
            return null; // User not found
        }

        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            logger.LogWarning("Authentication failed: Invalid password for user {UserId}, Tag: {Tag}.", user.Id,
                user.Tag);
            return null; // Invalid password
        }

        logger.LogInformation("User authenticated successfully: {UserId}, Tag: {Tag}", user.Id, user.Tag);
        return user;
    }

    /// <summary>
    /// Retrieves a user by their unique identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A <see cref="User"/> object representing the user, or null if the user is not found.</returns>
    public async Task<User?> GetUserByIdAsync(Guid userId)
    {
        return await context.Users.FindAsync(userId);
    }

    /// <summary>
    /// Searches for users based on a query string.
    /// </summary>
    /// <param name="query">The search query used to find users.</param>
    /// <returns>A collection of <see cref="User"/> objects matching the search criteria.</returns>
    public async Task<IEnumerable<User>> SearchUsersAsync(string query)
    {
        var searchTerm = query.Trim().ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return [];
        }

        return await context.Users
            .Where(u => u.Name.ToLower().Contains(searchTerm) || u.Tag.ToLower().Contains(searchTerm))
            .OrderByDescending(u => u.Tag.ToLower() == searchTerm)
            .ThenBy(u => u.Name)
            .Take(20)
            .ToListAsync();
    }
}