using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UserAccountService.Data;
using UserAccountService.Models;
using UserAccountService.Models.Configuration;

namespace UserAccountService.Services;

/// <summary>
/// Provides methods for generating, refreshing, and invalidating tokens.
/// </summary>
public class TokenService(UserAccountDbContext context, IOptions<JwtSettings> jwtSettings, ILogger<TokenService> logger)
    : ITokenService
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    /// <summary>
    /// Generates a new access token and refresh token for the specified user.
    /// </summary>
    /// <param name="user">The user for whom the tokens are being generated.</param>
    /// <returns>A tuple containing the access token as a string and the refresh token as a <see cref="RefreshToken"/>.</returns>
    public (string accessToken, RefreshToken refreshToken) GenerateTokens(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

        var jwtId = Guid.NewGuid().ToString().Trim();

        var claims = new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, jwtId),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("tag", user.Tag)
        ]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claims,
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var accessToken = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            ExpiryDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays),
            UserId = user.Id,
            JwtId = jwtId,
            Used = false,
            Invalidated = false
        };

        context.RefreshTokens.Add(refreshToken);
        context.SaveChanges();

        logger.LogInformation("Generated new tokens for User ID: {UserId}. Access Token JTI: {JwtId}", user.Id, jwtId);
        return (accessToken, refreshToken);
    }

    /// <summary>
    /// Refreshes the access token using the provided old refresh token string.
    /// </summary>
    /// <param name="oldRefreshTokenString">The string representation of the old refresh token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a tuple with the new access token as a string and the new refresh token as a <see cref="RefreshToken"/>, or null if the refresh operation fails.</returns>
    public async Task<(string? newAccessToken, RefreshToken? newRefreshToken)> RefreshAccessTokenAsync(
        string oldRefreshTokenString)
    {
        var storedToken = await context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == oldRefreshTokenString);

        if (storedToken == null)
        {
            logger.LogWarning("Refresh token not found: {RefreshToken}", oldRefreshTokenString);
            return (null, null);
        }

        if (storedToken.Used)
        {
            logger.LogWarning("Refresh token already used: {RefreshToken} for User ID: {UserId}", oldRefreshTokenString,
                storedToken.UserId);
            return (null, null);
        }

        if (storedToken.Invalidated)
        {
            logger.LogWarning("Refresh token has been invalidated: {RefreshToken} for User ID: {UserId}",
                oldRefreshTokenString, storedToken.UserId);
            return (null, null);
        }

        if (storedToken.ExpiryDate <= DateTime.UtcNow)
        {
            logger.LogWarning("Refresh token expired: {RefreshToken} for User ID: {UserId}", oldRefreshTokenString,
                storedToken.UserId);
            return (null, null);
        }

        storedToken.Used = true;
        context.RefreshTokens.Update(storedToken);

        var (newAccessToken, newGeneratedRefreshToken) = GenerateTokens(storedToken.User!);

        await context.SaveChangesAsync();

        logger.LogInformation(
            "Successfully refreshed tokens for User ID: {UserId} using old refresh token: {OldRefreshToken}",
            storedToken.UserId, oldRefreshTokenString);
        return (newAccessToken, newGeneratedRefreshToken);
    }

    /// <summary>
    /// Invalidates the specified refresh token.
    /// </summary>
    /// <param name="refreshTokenString">The string representation of the refresh token to invalidate.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is a boolean indicating whether the refresh token was successfully invalidated.</returns>
    public async Task<bool> InvalidateRefreshTokenAsync(string refreshTokenString)
    {
        var storedToken = await context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshTokenString);

        if (storedToken == null || storedToken.Used || storedToken.Invalidated)
        {
            logger.LogWarning(
                "Attempt to invalidate an invalid, already invalidated, or used refresh token: {RefreshToken}",
                refreshTokenString);
            return false;
        }

        storedToken.Invalidated = true;
        context.RefreshTokens.Update(storedToken);
        await context.SaveChangesAsync();

        logger.LogInformation("Refresh token invalidated successfully: {RefreshToken} for User ID: {UserId}",
            refreshTokenString, storedToken.UserId);
        return true;
    }
}