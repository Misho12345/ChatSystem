namespace UserAccountService.Models.DTOs;

/// <summary>
/// Represents the response returned after a successful login.
/// </summary>
public class LoginResponseDto
{
    /// <summary>
    /// The access token issued upon successful authentication.
    /// </summary>
    /// <remarks>
    /// The access token is used to authorize requests to protected resources.
    /// </remarks>
    /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...</example>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// The refresh token issued to obtain a new access token when the current one expires.
    /// </summary>
    /// <remarks>
    /// The refresh token is a secure string and must be kept confidential.
    /// </remarks>
    /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...</example>
    public string RefreshToken { get; set; } = string.Empty;
}