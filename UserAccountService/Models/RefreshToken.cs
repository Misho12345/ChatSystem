using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserAccountService.Models;

/// <summary>
/// Represents a refresh token used for authentication and session management.
/// </summary>
public sealed class RefreshToken
{
    /// <summary>
    /// Gets or sets the unique token string.
    /// </summary>
    [MaxLength(8192)]
    [Key]
    public required string Token { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the associated JWT.
    /// </summary>
    [MaxLength(255)]
    public required string JwtId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user associated with the refresh token.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the user associated with the refresh token.
    /// </summary>
    [ForeignKey("UserId")]
    public User? User { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the refresh token was created.
    /// Defaults to the current UTC time.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the expiry date and time of the refresh token.
    /// </summary>
    public DateTime ExpiryDate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the refresh token has been used.
    /// </summary>
    public bool Used { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the refresh token has been invalidated.
    /// </summary>
    public bool Invalidated { get; set; }
}