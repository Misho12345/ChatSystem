using UserAccountService.Data;
using UserAccountService.Models;
using UserAccountService.Models.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace UserAccountService.Services;

public class TokenService(UserAccountDbContext context, IOptions<JwtSettings> jwtSettings, ILogger<TokenService> logger)
    : ITokenService
{
    private readonly JwtSettings _jwtSettings = jwtSettings.Value;

    public (string accessToken, RefreshToken refreshToken) GenerateTokens(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new("tag", user.Tag),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var accessTokenSecurityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            signingCredentials: creds
        );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(accessTokenSecurityToken);

        var newRefreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            JwtId = accessTokenSecurityToken.Id,
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow,
            ExpiryDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays)
        };

        context.RefreshTokens.Add(newRefreshToken);

        logger.LogInformation("Generated new tokens for User ID: {UserId}. Access Token JTI: {Jti}", user.Id,
            newRefreshToken.JwtId);
        return (accessToken, newRefreshToken);
    }

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

        var (newAccessToken, newGeneratedRefreshToken) = GenerateTokens(storedToken.User);

        logger.LogInformation(
            "Successfully refreshed tokens for User ID: {UserId} using old refresh token: {OldRefreshToken}",
            storedToken.UserId, oldRefreshTokenString);
        return (newAccessToken, newGeneratedRefreshToken);
    }

    public async Task<bool> InvalidateRefreshTokenAsync(string refreshTokenString)
    {
        var storedToken = await context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == refreshTokenString);

        if (storedToken == null || storedToken.Invalidated || storedToken.Used)
        {
            logger.LogWarning(
                "Attempt to invalidate an invalid, already invalidated, or used refresh token: {RefreshToken}",
                refreshTokenString);
            return false;
        }

        storedToken.Invalidated = true;
        context.RefreshTokens.Update(storedToken);

        logger.LogInformation("Refresh token invalidated successfully: {RefreshToken} for User ID: {UserId}",
            refreshTokenString, storedToken.UserId);
        return true;
    }
}