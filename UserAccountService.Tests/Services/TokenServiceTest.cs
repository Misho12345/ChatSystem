using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using UserAccountService.Data;
using UserAccountService.Models;
using UserAccountService.Models.Configuration;
using UserAccountService.Services;
using Xunit;

namespace UserAccountService.Tests.Services;

/// <summary>
/// Unit tests for the TokenService class.
/// </summary>
public class TokenServiceTest : IDisposable
{
    private readonly UserAccountDbContext _context;
    private readonly TokenService _tokenService;
    private readonly User _testUser;

    /// <summary>
    /// Initializes the TokenServiceTest class and sets up mock dependencies and test data.
    /// </summary>
    public TokenServiceTest()
    {
        var options = new DbContextOptionsBuilder<UserAccountDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new UserAccountDbContext(options);

        var jwtSettings = new JwtSettings
        {
            Secret = "this_is_a_very_long_secret_key_with_at_least_32_chars_for_testing",
            Issuer = "test_issuer",
            Audience = "test_audience",
            ExpiryMinutes = 15,
            RefreshTokenExpiryDays = 7
        };
        var mockOptions = Options.Create(jwtSettings);
        var mockLogger = new Mock<ILogger<TokenService>>();

        _tokenService = new TokenService(_context, mockOptions, mockLogger.Object);

        _testUser = new User
        {
            Id = Guid.NewGuid(),
            Name = "Test User",
            Tag = "test#1234",
            Email = "test@example.com",
            PasswordHash = "hashedpassword"
        };
        _context.Users.Add(_testUser);
        _context.SaveChanges();
    }

    /// <summary>
    /// Cleans up resources used by the test class.
    /// </summary>
    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Tests that GenerateTokens returns valid access and refresh tokens and saves the refresh token in the database.
    /// </summary>
    [Fact]
    public void GenerateTokens_ReturnsValidTokensAndSavesRefreshToken()
    {
        var (accessToken, refreshToken) = _tokenService.GenerateTokens(_testUser);

        Assert.NotNull(accessToken);
        Assert.NotNull(refreshToken);
        Assert.False(string.IsNullOrEmpty(accessToken));
        Assert.False(string.IsNullOrEmpty(refreshToken.Token));
        Assert.Equal(_testUser.Id, refreshToken.UserId);
        Assert.False(refreshToken.Used);
        Assert.False(refreshToken.Invalidated);
        Assert.True(refreshToken.ExpiryDate > DateTime.UtcNow);

        Assert.Contains(refreshToken, _context.RefreshTokens);
    }

    /// <summary>
    /// Tests that RefreshAccessTokenAsync returns new tokens when a valid refresh token is provided.
    /// </summary>
    [Fact]
    public async Task RefreshAccessTokenAsync_WithValidRefreshToken_ReturnsNewTokens()
    {
        var (_, oldRefreshToken) = _tokenService.GenerateTokens(_testUser);
        await _context.SaveChangesAsync();

        var (newAccessToken, newRefreshToken) =
            await _tokenService.RefreshAccessTokenAsync(oldRefreshToken.Token);

        Assert.NotNull(newAccessToken);
        Assert.NotNull(newRefreshToken);
        Assert.False(string.IsNullOrEmpty(newAccessToken));
        Assert.False(string.IsNullOrEmpty(newRefreshToken.Token));

        var updatedOldToken = await _context.RefreshTokens.FindAsync(oldRefreshToken.Token);
        Assert.NotNull(updatedOldToken);
        Assert.True(updatedOldToken.Used);
        Assert.False(updatedOldToken.Invalidated);

        Assert.Contains(newRefreshToken, _context.RefreshTokens);
    }

    /// <summary>
    /// Tests that RefreshAccessTokenAsync returns null when an invalid refresh token is provided.
    /// </summary>
    [Fact]
    public async Task RefreshAccessTokenAsync_WithInvalidRefreshToken_ReturnsNull()
    {
        var (newAccessToken, newRefreshToken) =
            await _tokenService.RefreshAccessTokenAsync("invalid_token");

        Assert.Null(newAccessToken);
        Assert.Null(newRefreshToken);
    }

    /// <summary>
    /// Tests that RefreshAccessTokenAsync returns null when a used refresh token is provided.
    /// </summary>
    [Fact]
    public async Task RefreshAccessTokenAsync_WithUsedRefreshToken_ReturnsNull()
    {
        var (_, oldRefreshToken) = _tokenService.GenerateTokens(_testUser);
        oldRefreshToken.Used = true;
        await _context.SaveChangesAsync();

        var (newAccessToken, newRefreshToken) =
            await _tokenService.RefreshAccessTokenAsync(oldRefreshToken.Token);

        Assert.Null(newAccessToken);
        Assert.Null(newRefreshToken);
    }

    /// <summary>
    /// Tests that RefreshAccessTokenAsync returns null when an invalidated refresh token is provided.
    /// </summary>
    [Fact]
    public async Task RefreshAccessTokenAsync_WithInvalidatedRefreshToken_ReturnsNull()
    {
        var (_, oldRefreshToken) = _tokenService.GenerateTokens(_testUser);
        oldRefreshToken.Invalidated = true;
        await _context.SaveChangesAsync();

        var (newAccessToken, newRefreshToken) =
            await _tokenService.RefreshAccessTokenAsync(oldRefreshToken.Token);

        Assert.Null(newAccessToken);
        Assert.Null(newRefreshToken);
    }

    /// <summary>
    /// Tests that RefreshAccessTokenAsync returns null when an expired refresh token is provided.
    /// </summary>
    [Fact]
    public async Task RefreshAccessTokenAsync_WithExpiredRefreshToken_ReturnsNull()
    {
        var (_, oldRefreshToken) = _tokenService.GenerateTokens(_testUser);
        oldRefreshToken.ExpiryDate = DateTime.UtcNow.AddMinutes(-1);
        await _context.SaveChangesAsync();

        var (newAccessToken, newRefreshToken) =
            await _tokenService.RefreshAccessTokenAsync(oldRefreshToken.Token);

        Assert.Null(newAccessToken);
        Assert.Null(newRefreshToken);
    }

    /// <summary>
    /// Tests that InvalidateRefreshTokenAsync invalidates a valid refresh token.
    /// </summary>
    [Fact]
    public async Task InvalidateRefreshTokenAsync_WithValidRefreshToken_InvalidatesToken()
    {
        var (_, refreshToken) = _tokenService.GenerateTokens(_testUser);
        await _context.SaveChangesAsync();

        var result = await _tokenService.InvalidateRefreshTokenAsync(refreshToken.Token);

        Assert.True(result);
        var updatedToken = await _context.RefreshTokens.FindAsync(refreshToken.Token);
        Assert.NotNull(updatedToken);
        Assert.True(updatedToken.Invalidated);
    }

    /// <summary>
    /// Tests that InvalidateRefreshTokenAsync returns false when an invalid refresh token is provided.
    /// </summary>
    [Fact]
    public async Task InvalidateRefreshTokenAsync_WithInvalidRefreshToken_ReturnsFalse()
    {
        var result = await _tokenService.InvalidateRefreshTokenAsync("non_existent_token");

        Assert.False(result);
    }

    /// <summary>
    /// Tests that InvalidateRefreshTokenAsync returns false when an already invalidated refresh token is provided.
    /// </summary>
    [Fact]
    public async Task InvalidateRefreshTokenAsync_WithAlreadyInvalidatedRefreshToken_ReturnsFalse()
    {
        var (_, refreshToken) = _tokenService.GenerateTokens(_testUser);
        refreshToken.Invalidated = true;
        await _context.SaveChangesAsync();

        var result = await _tokenService.InvalidateRefreshTokenAsync(refreshToken.Token);

        Assert.False(result);
    }

    /// <summary>
    /// Tests that InvalidateRefreshTokenAsync returns false when a used refresh token is provided.
    /// </summary>
    [Fact]
    public async Task InvalidateRefreshTokenAsync_WithUsedRefreshToken_ReturnsFalse()
    {
        var (_, refreshToken) = _tokenService.GenerateTokens(_testUser);
        refreshToken.Used = true;
        await _context.SaveChangesAsync();

        var result = await _tokenService.InvalidateRefreshTokenAsync(refreshToken.Token);

        Assert.False(result);
    }
}