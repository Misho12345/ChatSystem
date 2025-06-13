using System.ComponentModel.DataAnnotations;

namespace UserAccountService.Models.DTOs;

public class RegisterRequestDto
{
    public string Name { get; set; } = string.Empty;

    [RegularExpression(@"^[a-zA-Z0-9_]{1,45}#[0-9]{4}$",
        ErrorMessage =
            "Tag must be in the format 'username#1234' and can contain letters, numbers, and underscores. Username part must be 1–45 characters, followed by a # and a 4-digit number.")]
    public string Tag { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}