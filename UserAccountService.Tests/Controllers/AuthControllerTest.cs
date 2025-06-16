using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UserAccountService.Controllers;
using UserAccountService.Services;
using UserAccountService.Models.DTOs;
using UserAccountService.Models;
using UserAccountService.Data;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace UserAccountService.Tests.Controllers;

/// <summary>
/// Unit tests for the AuthController class.
/// </summary>
public class AuthControllerTest : IDisposable
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly Mock<ITokenService> _mockTokenService;
    private readonly UserAccountDbContext _context;
    private readonly AuthController _controller;

    /// <summary>
    /// Initializes a new instance of the AuthControllerTest class.
    /// Sets up mocks and an in-memory database for testing.
    /// </summary>
    public AuthControllerTest()
    {
        _mockUserService = new Mock<IUserService>();
        _mockTokenService = new Mock<ITokenService>();

        var options = new DbContextOptionsBuilder<UserAccountDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new UserAccountDbContext(options);

        _ = new Mock<ILogger<AuthController>>();

        _controller = new AuthController(_mockUserService.Object, _mockTokenService.Object, _context);
    }

    /// <summary>
    /// Cleans up resources after each test.
    /// Ensures the in-memory database is deleted and disposed.
    /// </summary>
    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    /// <summary>
    /// Tests the Register method with valid data.
    /// Verifies that a CreatedAtActionResult is returned and the user is registered successfully.
    /// </summary>
    [Fact]
    public async Task Register_WithValidData_ReturnsCreatedAtAction()
    {
        var request = new RegisterRequestDto
        {
            Name = "Test User",
            Tag = "test#1234",
            Email = "test@example.com",
            Password = "Password123!"
        };
        var user = new User
        {
            Id = Guid.NewGuid(), Name = request.Name, Tag = request.Tag, Email = request.Email,
            PasswordHash = "hashed_password"
        };

        _mockUserService.Setup(s => s.RegisterUserAsync(request.Name, request.Tag, request.Email, request.Password))
            .ReturnsAsync(user);

        var result = await _controller.Register(request);

        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(UsersController.GetUserById), createdAtActionResult.ActionName);
        Assert.Equal("Users", createdAtActionResult.ControllerName);
        var returnedUserDto = Assert.IsType<UserDto>(createdAtActionResult.Value);
        Assert.Equal(user.Id, returnedUserDto.Id);
        _mockUserService.Verify(
            s => s.RegisterUserAsync(request.Name, request.Tag, request.Email, request.Password), Times.Once);
    }

    /// <summary>
    /// Tests the Register method with an existing user.
    /// Verifies that a BadRequestObjectResult is returned with an appropriate error message.
    /// </summary>
    [Fact]
    public async Task Register_WithExistingUser_ReturnsBadRequest()
    {
        var request = new RegisterRequestDto
        {
            Name = "Test User",
            Tag = "existing#user",
            Email = "existing@example.com",
            Password = "Password123!"
        };

        _mockUserService.Setup(s => s.RegisterUserAsync(request.Name, request.Tag, request.Email, request.Password))
            .ReturnsAsync((User)null);

        var result = await _controller.Register(request);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("User with this tag or email already exists.", badRequestResult.Value);
        _mockUserService.Verify(
            s => s.RegisterUserAsync(request.Name, request.Tag, request.Email, request.Password), Times.Once);
    }

    /// <summary>
    /// Tests the Register method with invalid model state.
    /// Verifies that a BadRequestObjectResult is returned and no user registration is attempted.
    /// </summary>
    [Fact]
    public async Task Register_WithInvalidModelState_ReturnsBadRequest()
    {
        var request = new RegisterRequestDto { Name = "", Tag = "", Email = "invalid", Password = "" };
        _controller.ModelState.AddModelError("Name", "Name is required");

        var result = await _controller.Register(request);

        Assert.IsType<BadRequestObjectResult>(result);
        _mockUserService.Verify(
            s => s.RegisterUserAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>()), Times.Never);
    }

    /// <summary>
    /// Tests the Login method with valid credentials.
    /// Verifies that an OkObjectResult is returned with access and refresh tokens.
    /// </summary>
    [Fact]
    public async Task Login_WithValidCredentials_ReturnsOkWithTokens()
    {
        var request = new LoginRequestDto { TagOrEmail = "test#1234", Password = "Password123!" };
        var user = new User
        {
            Id = Guid.NewGuid(), Name = "Test User", Tag = "test#1234", Email = "test@example.com",
            PasswordHash = "hashed_password"
        };
        var accessToken = "testAccessToken";
        var refreshToken = new RefreshToken
        {
            Token = "testRefreshToken", JwtId = "jti", UserId = user.Id, ExpiryDate = DateTime.UtcNow.AddDays(7)
        };

        _mockUserService.Setup(s => s.AuthenticateUserAsync(request.TagOrEmail, request.Password))
            .ReturnsAsync(user);
        _mockTokenService.Setup(s => s.GenerateTokens(user))
            .Returns((accessToken, refreshToken));

        var result = await _controller.Login(request);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var loginResponse = Assert.IsType<LoginResponseDto>(okResult.Value);
        Assert.Equal(accessToken, loginResponse.AccessToken);
        Assert.Equal(refreshToken.Token, loginResponse.RefreshToken);
        _mockUserService.Verify(s => s.AuthenticateUserAsync(request.TagOrEmail, request.Password), Times.Once);
        _mockTokenService.Verify(s => s.GenerateTokens(user), Times.Once);
    }

    /// <summary>
    /// Tests the Login method with invalid credentials.
    /// Verifies that an UnauthorizedObjectResult is returned with an appropriate error message.
    /// </summary>
    [Fact]
    public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
    {
        var request = new LoginRequestDto { TagOrEmail = "wrong#user", Password = "wrongpassword" };
        _mockUserService.Setup(s => s.AuthenticateUserAsync(request.TagOrEmail, request.Password))
            .ReturnsAsync((User)null);

        var result = await _controller.Login(request);

        var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
        Assert.Equal("Invalid credentials.", unauthorizedResult.Value);
        _mockUserService.Verify(s => s.AuthenticateUserAsync(request.TagOrEmail, request.Password), Times.Once);
        _mockTokenService.Verify(s => s.GenerateTokens(It.IsAny<User>()), Times.Never);
    }

    /// <summary>
    /// Tests the Login method with invalid model state.
    /// Verifies that a BadRequestObjectResult is returned and no authentication is attempted.
    /// </summary>
    [Fact]
    public async Task Login_WithInvalidModelState_ReturnsBadRequest()
    {
        var request = new LoginRequestDto { TagOrEmail = "", Password = "" };
        _controller.ModelState.AddModelError("TagOrEmail", "Identifier is required");

        var result = await _controller.Login(request);

        Assert.IsType<BadRequestObjectResult>(result);
        _mockUserService.Verify(s => s.AuthenticateUserAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    /// <summary>
    /// Tests the Refresh method with a valid refresh token.
    /// Verifies that an OkObjectResult is returned with new access and refresh tokens.
    /// </summary>
    [Fact]
    public async Task Refresh_WithValidRefreshToken_ReturnsOkWithNewTokens()
    {
        var request = new RefreshRequestDto { RefreshToken = "oldRefreshToken" };
        var newAccessToken = "newAccessToken";
        var newRefreshToken = new RefreshToken
        {
            Token = "newRefreshToken", JwtId = "new_jti", UserId = Guid.NewGuid(),
            ExpiryDate = DateTime.UtcNow.AddDays(7)
        };

        _mockTokenService.Setup(s => s.RefreshAccessTokenAsync(request.RefreshToken))
            .ReturnsAsync((newAccessToken, newRefreshToken));

        var result = await _controller.Refresh(request);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var loginResponse = Assert.IsType<LoginResponseDto>(okResult.Value);
        Assert.Equal(newAccessToken, loginResponse.AccessToken);
        Assert.Equal(newRefreshToken.Token, loginResponse.RefreshToken);
        _mockTokenService.Verify(s => s.RefreshAccessTokenAsync(request.RefreshToken), Times.Once);
    }

    /// <summary>
    /// Tests the Refresh method with an invalid refresh token.
    /// Verifies that an UnauthorizedObjectResult is returned with an appropriate error message.
    /// </summary>
    [Fact]
    public async Task Refresh_WithInvalidRefreshToken_ReturnsUnauthorized()
    {
        var request = new RefreshRequestDto { RefreshToken = "invalidRefreshToken" };
        _mockTokenService.Setup(s => s.RefreshAccessTokenAsync(request.RefreshToken))
            .ReturnsAsync((null, null));

        var result = await _controller.Refresh(request);

        var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
        Assert.Equal("Invalid refresh token.", unauthorizedResult.Value);
        _mockTokenService.Verify(s => s.RefreshAccessTokenAsync(request.RefreshToken), Times.Once);
    }

    /// <summary>
    /// Tests the Refresh method with invalid model state.
    /// Verifies that a BadRequestObjectResult is returned and no token refresh is attempted.
    /// </summary>
    [Fact]
    public async Task Refresh_WithInvalidModelState_ReturnsBadRequest()
    {
        var request = new RefreshRequestDto { RefreshToken = "" };
        _controller.ModelState.AddModelError("RefreshToken", "Refresh token is required");

        var result = await _controller.Refresh(request);

        Assert.IsType<BadRequestObjectResult>(result);
        _mockTokenService.Verify(s => s.RefreshAccessTokenAsync(It.IsAny<string>()), Times.Never);
    }

    /// <summary>
    /// Tests the Logout method with a valid refresh token.
    /// Verifies that an OkObjectResult is returned with a success message.
    /// </summary>
    [Fact]
    public async Task Logout_WithValidRefreshToken_ReturnsOk()
    {
        var request = new RefreshRequestDto { RefreshToken = "validRefreshToken" };
        _mockTokenService.Setup(s => s.InvalidateRefreshTokenAsync(request.RefreshToken))
            .ReturnsAsync(true);

        var result = await _controller.Logout(request);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Logged out successfully.", okResult.Value);
        _mockTokenService.Verify(s => s.InvalidateRefreshTokenAsync(request.RefreshToken), Times.Once);
    }

    /// <summary>
    /// Tests the Logout method with an invalid refresh token.
    /// Verifies that a BadRequestObjectResult is returned with an appropriate error message.
    /// </summary>
    [Fact]
    public async Task Logout_WithInvalidRefreshToken_ReturnsBadRequest()
    {
        var request = new RefreshRequestDto { RefreshToken = "invalidRefreshToken" };
        _mockTokenService.Setup(s => s.InvalidateRefreshTokenAsync(request.RefreshToken))
            .ReturnsAsync(false);

        var result = await _controller.Logout(request);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Invalid refresh token or already invalidated.", badRequestResult.Value);
        _mockTokenService.Verify(s => s.InvalidateRefreshTokenAsync(request.RefreshToken), Times.Once);
    }

    /// <summary>
    /// Tests the Logout method with invalid model state.
    /// Verifies that a BadRequestObjectResult is returned and no token invalidation is attempted.
    /// </summary>
    [Fact]
    public async Task Logout_WithInvalidModelState_ReturnsBadRequest()
    {
        var request = new RefreshRequestDto { RefreshToken = "" };
        _controller.ModelState.AddModelError("RefreshToken", "Refresh token is required");

        var result = await _controller.Logout(request);

        Assert.IsType<BadRequestObjectResult>(result);
        _mockTokenService.Verify(s => s.InvalidateRefreshTokenAsync(It.IsAny<string>()), Times.Never);
    }
}