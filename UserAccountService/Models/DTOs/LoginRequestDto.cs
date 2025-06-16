namespace UserAccountService.Models.DTOs;

/// <summary>
/// Represents a request to log in to the system.
/// </summary>
public class LoginRequestDto
{
    /// <summary>
    /// The user's tag or email used for authentication.
    /// </summary>
    /// <remarks>
    /// This field can contain either:
    /// - A tag in the format 'username#1234'.
    /// - A valid email address.
    /// </remarks>
    /// <example>john.doe#1234</example>
    /// <example>john.doe@example.com</example>
    public string TagOrEmail { get; set; } = string.Empty;

    /// <summary>
    /// The password associated with the user's account.
    /// </summary>
    /// <remarks>
    /// The password must match the one set during registration.
    /// </remarks>
    /// <example>SecurePassword123!</example>
    public string Password { get; set; } = string.Empty;
}