using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserAccountService.Data;
using UserAccountService.Models;
using UserAccountService.Services;

namespace UserAccountService.Tests.Services;

/// <summary>
/// Unit tests for the UserService class.
/// </summary>
public class UserServiceTest : IDisposable
{
    private readonly Mock<ILogger<UserService>> _mockLogger;
    private readonly UserAccountDbContext _context;
    private readonly UserService _userService;

    /// <summary>
    /// Initializes the UserServiceTest class and sets up mock dependencies and test data.
    /// </summary>
    public UserServiceTest()
    {
        var dbContextOptions =
            new DbContextOptionsBuilder<UserAccountDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

        _mockLogger = new Mock<ILogger<UserService>>();
        _context = new UserAccountDbContext(dbContextOptions);
        _userService = new UserService(_context, _mockLogger.Object);

        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
    }

    /// <summary>
    /// Cleans up resources used by the test class.
    /// </summary>
    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Adds a test user to the database.
    /// </summary>
    /// <param name="name">The name of the user.</param>
    /// <param name="tag">The tag of the user.</param>
    /// <param name="email">The email of the user.</param>
    /// <param name="password">The password of the user.</param>
    /// <returns>The added user.</returns>
    private async Task<User> AddTestUser(string name, string tag, string email, string password)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = name,
            Tag = tag,
            Email = email.ToLowerInvariant(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            CreatedAt = DateTime.UtcNow
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    #region RegisterUserAsync Tests

    /// <summary>
    /// Tests that RegisterUserAsync registers a user when the user does not already exist.
    /// </summary>
    [Fact]
    public async Task RegisterUserAsync_ShouldRegisterUser_WhenUserDoesNotExist()
    {
        const string name = "Test User";
        const string tag = "testuser";
        const string email = "test@example.com";
        const string password = "Password123!";

        var registeredUser = await _userService.RegisterUserAsync(name, tag, email, password);

        Assert.NotNull(registeredUser);
        Assert.Equal(name, registeredUser.Name);
        Assert.Equal(tag, registeredUser.Tag);
        Assert.Equal(email.ToLowerInvariant(), registeredUser.Email);
        Assert.True(BCrypt.Net.BCrypt.Verify(password, registeredUser.PasswordHash));

        var userInDb = await _context.Users.FindAsync(registeredUser.Id);
        Assert.NotNull(userInDb);
        Assert.Equal(registeredUser.Id, userInDb.Id);

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) =>
                    v.ToString()!
                        .Contains($"User registered successfully: {registeredUser.Id}, Tag: {registeredUser.Tag}")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    /// <summary>
    /// Tests that RegisterUserAsync returns null when a user with the same tag already exists.
    /// </summary>
    [Fact]
    public async Task RegisterUserAsync_ShouldReturnNull_WhenUserWithSameTagExists()
    {
        _ = await AddTestUser("Existing User", "existingtag", "existing@example.com", "Password123!");
        const string name = "New User";
        const string tag = "existingtag";
        const string email = "new@example.com";
        const string password = "NewPassword123!";

        var registeredUser = await _userService.RegisterUserAsync(name, tag, email, password);

        Assert.Null(registeredUser);
        Assert.Equal(1, await _context.Users.CountAsync());

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) =>
                    v.ToString()!
                        .Contains($"Registration attempt failed: Tag '{tag}' or Email '{email.ToLowerInvariant()}' already exists.")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    /// <summary>
    /// Tests that RegisterUserAsync returns null when a user with the same email already exists.
    /// </summary>
    [Fact]
    public async Task RegisterUserAsync_ShouldReturnNull_WhenUserWithSameEmailExists()
    {
        _ = await AddTestUser("Existing User", "existingtag", "existing@example.com", "Password123!");
        const string name = "New User";
        const string tag = "newtag";
        const string email = "existing@example.com";
        const string password = "NewPassword123!";

        var registeredUser = await _userService.RegisterUserAsync(name, tag, email, password);

        Assert.Null(registeredUser);
        Assert.Equal(1, await _context.Users.CountAsync());

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) =>
                    v.ToString()!
                        .Contains($"Registration attempt failed: Tag '{tag}' or Email '{email.ToLowerInvariant()}' already exists.")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    #endregion
    
    #region AuthenticateUserAsync Tests

    /// <summary>
    /// Tests that AuthenticateUserAsync returns the user when credentials are correct.
    /// </summary>
    [Fact]
    public async Task AuthenticateUserAsync_ShouldReturnUser_WhenCredentialsAreCorrect()
    {
        const string email = "test@example.com";
        const string password = "Password123!";
        var user = await AddTestUser("Test User", "testuser", email, password);

        var authenticatedUser = await _userService.AuthenticateUserAsync(email, password);

        Assert.NotNull(authenticatedUser);
        Assert.Equal(user.Id, authenticatedUser.Id);
    }

    /// <summary>
    /// Tests that AuthenticateUserAsync returns null for a non-existent user.
    /// </summary>
    [Fact]
    public async Task AuthenticateUserAsync_ShouldReturnNull_ForNonExistentUser()
    {
        var authenticatedUser = await _userService.AuthenticateUserAsync("nonexistent@example.com", "somepassword");

        Assert.Null(authenticatedUser);
    }

    /// <summary>
    /// Tests that AuthenticateUserAsync returns null for an incorrect password.
    /// </summary>
    [Fact]
    public async Task AuthenticateUserAsync_ShouldReturnNull_ForIncorrectPassword()
    {
        const string email = "test@example.com";
        const string correctPassword = "Password123!";
        const string incorrectPassword = "WrongPassword!";
        await AddTestUser("Test User", "testuser", email, correctPassword);

        var authenticatedUser = await _userService.AuthenticateUserAsync(email, incorrectPassword);

        Assert.Null(authenticatedUser);
    }

    #endregion

    #region GetUserByIdAsync Tests

    /// <summary>
    /// Tests that GetUserByIdAsync returns the correct user when the user exists.
    /// </summary>
    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnUser_WhenUserExists()
    {
        var user = await AddTestUser("Test User", "testuser", "test@example.com", "Password123!");

        var foundUser = await _userService.GetUserByIdAsync(user.Id);

        Assert.NotNull(foundUser);
        Assert.Equal(user.Id, foundUser.Id);
        Assert.Equal(user.Name, foundUser.Name);
    }

    /// <summary>
    /// Tests that GetUserByIdAsync returns null when the user does not exist.
    /// </summary>
    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnNull_WhenUserDoesNotExist()
    {
        var foundUser = await _userService.GetUserByIdAsync(Guid.NewGuid());

        Assert.Null(foundUser);
    }

    #endregion

    #region SearchUsersAsync Tests

    /// <summary>
    /// Tests that SearchUsersAsync returns users matching the search query.
    /// </summary>
    [Fact]
    public async Task SearchUsersAsync_ShouldReturnMatchingUsers()
    {
        await AddTestUser("Alice Smith", "alice", "alice@example.com", "pass");
        await AddTestUser("Bob Johnson", "bobby", "bob@example.com", "pass");
        await AddTestUser("Charlie Brown", "charlie", "charlie@example.com", "pass");

        var results = await _userService.SearchUsersAsync("li");

        Assert.NotNull(results);
        var enumerable = results as User[] ?? results.ToArray();
        Assert.Equal(2, enumerable.Length);
        Assert.Contains(enumerable, u => u.Name == "Alice Smith");
        Assert.Contains(enumerable, u => u.Name == "Charlie Brown");
    }

    /// <summary>
    /// Tests that SearchUsersAsync returns an empty list when no users match the query.
    /// </summary>
    [Fact]
    public async Task SearchUsersAsync_ShouldReturnEmpty_WhenNoMatches()
    {
        await AddTestUser("Alice", "alice", "alice@example.com", "pass");
        
        var results = await _userService.SearchUsersAsync("xyz");

        Assert.NotNull(results);
        Assert.Empty(results);
    }

    /// <summary>
    /// Tests that SearchUsersAsync is case-insensitive.
    /// </summary>
    [Fact]
    public async Task SearchUsersAsync_ShouldBeCaseInsensitive()
    {
        await AddTestUser("Test User", "TESTUSER", "test@example.com", "pass");

        var results = await _userService.SearchUsersAsync("test");
        Assert.Single(results);
        
        results = await _userService.SearchUsersAsync("Test");
        Assert.Single(results);
    }
    
    /// <summary>
    /// Tests that SearchUsersAsync returns an empty enumerable if the query is whitespace.
    /// </summary>
    [Fact]
    public async Task SearchUsersAsync_ShouldReturnEmpty_WhenQueryIsWhitespace()
    {
        await AddTestUser("Test User", "testuser", "test@example.com", "Password123!");

        var results = await _userService.SearchUsersAsync("   ");

        Assert.NotNull(results);
        Assert.Empty(results);
    }
    
    #endregion
}
