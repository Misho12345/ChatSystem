using Microsoft.AspNetCore.Mvc;
using UserAccountService.Data;
using UserAccountService.Models.DTOs;
using UserAccountService.Services;

namespace UserAccountService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IUserService userService, ITokenService tokenService, UserAccountDbContext context)
    : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestDto request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var user = await userService.RegisterUserAsync(request.Name, request.Tag, request.Email, request.Password);
        if (user == null) return BadRequest("User with this tag or email already exists.");

        // Optionally, log the user in immediately after registration
        // var (accessToken, refreshToken) = _tokenService.GenerateTokens(user);
        // await _context.SaveChangesAsync(); // Save the new refresh token
        // return CreatedAtAction(nameof(UsersController.GetUserById), "Users", new { id = user.Id }, 
        //    new LoginResponseDto { AccessToken = accessToken, RefreshToken = refreshToken.Token });

        return CreatedAtAction(nameof(UsersController.GetUserById), "Users", new { id = user.Id }, new UserDto(user));
    }

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