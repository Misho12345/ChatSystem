using UserAccountService.Data;
using UserAccountService.Models;
using Microsoft.EntityFrameworkCore;

namespace UserAccountService.Services;

public class UserService(UserAccountDbContext context, ILogger<UserService> logger) : IUserService
{
    public async Task<User?> RegisterUserAsync(string name, string tag, string email, string password)
    {
        if (await context.Users.AnyAsync(u => u.Tag == tag || u.Email == email))
        {
            logger.LogWarning("Registration attempt failed: Tag '{Tag}' or Email '{Email}' already exists.", tag, email);
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

    public async Task<User?> AuthenticateUserAsync(string tagOrEmail, string password)
    {
        var normalizedIdentifier = tagOrEmail.Trim().ToLowerInvariant();
        var user = await context.Users
           .FirstOrDefaultAsync(u => u.Tag.ToLower() == normalizedIdentifier || u.Email.ToLower() == normalizedIdentifier);

        if (user == null)
        {
            logger.LogWarning("Authentication failed: User not found for identifier '{Identifier}'.", tagOrEmail);
            return null; // User not found
        }

        if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            logger.LogWarning("Authentication failed: Invalid password for user {UserId}, Tag: {Tag}.", user.Id, user.Tag);
            return null; // Invalid password
        }

        logger.LogInformation("User authenticated successfully: {UserId}, Tag: {Tag}", user.Id, user.Tag);
        return user;
    }

    public async Task<User?> GetUserByIdAsync(Guid userId)
    {
        return await context.Users.FindAsync(userId);
    }

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