using Microsoft.AspNetCore.Mvc;
using UserAccountService.Data;
using UserAccountService.Models.DTOs;
using UserAccountService.Services;

namespace UserAccountService.Controllers;

/// <summary>
/// Handles authentication-related operations such as user registration, login, token refresh, and logout.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController(IUserService userService, ITokenService tokenService, UserAccountDbContext context)
    : ControllerBase
{
    /// <summary>
    /// Registers a new user in the system.
    /// </summary>
    /// <param name="request">The registration details provided by the user.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the registration operation.</returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestDto request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var user = await userService.RegisterUserAsync(request.Name, request.Tag, request.Email, request.Password);
        if (user == null) return BadRequest("User with this tag or email already exists.");

        return CreatedAtAction(nameof(UsersController.GetUserById), "Users", new { id = user.Id }, new UserDto(user));
    }

    /// <summary>
    /// Authenticates a user and generates access and refresh tokens.
    /// </summary>
    /// <param name="request">The login credentials provided by the user.</param>
    /// <returns>An <see cref="IActionResult"/> containing the generated tokens or an error message.</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var user = await userService.AuthenticateUserAsync(request.TagOrEmail, request.Password);
        if (user == null) return Unauthorized("Invalid credentials.");

        var (accessToken, refreshToken) = tokenService.GenerateTokens(user);
        await context.SaveChangesAsync();

        return Ok(new LoginResponseDto { AccessToken = accessToken, RefreshToken = refreshToken.Token });
    }

    /// <summary>
    /// Refreshes the access token using a valid refresh token.
    /// </summary>
    /// <param name="request">The refresh token provided by the user.</param>
    /// <returns>An <see cref="IActionResult"/> containing the new tokens or an error message.</returns>
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshRequestDto request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var (newAccessToken, newRefreshToken) = await tokenService.RefreshAccessTokenAsync(request.RefreshToken);

        if (string.IsNullOrEmpty(newAccessToken) || newRefreshToken == null)
        {
            return Unauthorized("Invalid refresh token.");
        }

        await context.SaveChangesAsync();
        return Ok(new LoginResponseDto { AccessToken = newAccessToken, RefreshToken = newRefreshToken.Token });
    }

    /// <summary>
    /// Logs out the user by invalidating the provided refresh token.
    /// </summary>
    /// <param name="request">The refresh token to invalidate.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the logout operation.</returns>
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(RefreshRequestDto request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var result = await tokenService.InvalidateRefreshTokenAsync(request.RefreshToken);
        if (!result) return BadRequest("Invalid refresh token or already invalidated.");

        await context.SaveChangesAsync();
        return Ok("Logged out successfully.");
    }
}