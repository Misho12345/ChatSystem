using UserAccountService.Models;

namespace UserAccountService.Services;

/// <summary>
/// Defines the contract for token management, including generation, refreshing, and invalidation of tokens.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Generates a new access token and refresh token for the specified user.
    /// </summary>
    /// <param name="user">The user for whom the tokens are being generated.</param>
    /// <returns>A tuple containing the access token as a string and the refresh token as a <see cref="RefreshToken"/>.</returns>
    (string accessToken, RefreshToken refreshToken) GenerateTokens(User user);

    /// <summary>
    /// Refreshes the access token using the provided old refresh token string.
    /// </summary>
    /// <param name="oldRefreshTokenString">The string representation of the old refresh token.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a tuple with the new access token as a string and the new refresh token as a <see cref="RefreshToken"/>, or null if the refresh operation fails.</returns>
    Task<(string? newAccessToken, RefreshToken? newRefreshToken)> RefreshAccessTokenAsync(string oldRefreshTokenString);

    /// <summary>
    /// Invalidates the specified refresh token.
    /// </summary>
    /// <param name="refreshTokenString">The string representation of the refresh token to invalidate.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is a boolean indicating whether the refresh token was successfully invalidated.</returns>
    Task<bool> InvalidateRefreshTokenAsync(string refreshTokenString);
}