using System.ComponentModel.DataAnnotations;

namespace UserAccountService.Models.DTOs;

/// <summary>
/// Represents a request to register a new user.
/// </summary>
public class RegisterRequestDto
{
    /// <summary>
    /// The desired username for the new account.
    /// </summary>
    /// <example>john.doe</example>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The unique tag associated with the user, in the format 'username#1234'.
    /// </summary>
    /// <remarks>
    /// The tag must consist of:
    /// - A username part containing 1–45 alphanumeric characters or underscores.
    /// - Followed by a '#' and a 4-digit number.
    /// </remarks>
    /// <example>john.doe#1234</example>
    [RegularExpression(@"^[a-zA-Z0-9_]{1,45}#[0-9]{4}$",
        ErrorMessage =
            "Tag must be in the format 'username#1234' and can contain letters, numbers, and underscores." +
            " Username part must be 1–45 characters, followed by a # and a 4-digit number.")]
    public string Tag { get; set; } = string.Empty;

    /// <summary>
    /// The user's email address.
    /// </summary>
    /// <remarks>
    /// The email address must be valid according to standard email format rules.
    /// </remarks>
    /// <example>john.doe@example.com</example>
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The password for the new account.
    /// </summary>
    /// <remarks>
    /// The password should be strong and secure to protect the user's account.
    /// </remarks>
    /// <example>SecurePassword123!</example>
    public string Password { get; set; } = string.Empty;
}