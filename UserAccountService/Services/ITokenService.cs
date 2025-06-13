using UserAccountService.Models;

namespace UserAccountService.Services;

public interface ITokenService
{
    (string accessToken, RefreshToken refreshToken) GenerateTokens(User user);
    Task<(string? newAccessToken, RefreshToken? newRefreshToken)> RefreshAccessTokenAsync(string oldRefreshTokenString);
    Task<bool> InvalidateRefreshTokenAsync(string refreshTokenString);
}