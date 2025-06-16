namespace UserAccountService.Models.DTOs;

/// <summary>
/// Represents a request to refresh an authentication token.
/// </summary>
public class RefreshRequestDto
{
    /// <summary>
    /// The refresh token used to obtain a new access token.
    /// </summary>
    /// <remarks>
    /// The refresh token is a secure string issued during authentication and must be valid.
    /// </remarks>
    /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...</example>
    public string RefreshToken { get; set; } = string.Empty;
}