using System.ComponentModel.DataAnnotations;

namespace UserAccountService.Models;

/// <summary>
/// Represents a user in the system, including their personal details, authentication information, and relationships.
/// </summary>
public sealed class User
{
    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the user.
    /// </summary>
    [MaxLength(100)]
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the unique tag associated with the user.
    /// </summary>
    [MaxLength(50)]
    public required string Tag { get; set; }

    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    [MaxLength(255)]
    [EmailAddress]
    public required string Email { get; set; }

    /// <summary>
    /// Gets or sets the hashed password for the user account.
    /// </summary>
    [MaxLength(255)]
    public required string PasswordHash { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the user was created.
    /// Defaults to the current UTC time.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the collection of friendships where the user is the first participant.
    /// </summary>
    public ICollection<Friendship> Friendships1 { get; set; } = new List<Friendship>();

    /// <summary>
    /// Gets or sets the collection of friendships where the user is the second participant.
    /// </summary>
    public ICollection<Friendship> Friendships2 { get; set; } = new List<Friendship>();

    /// <summary>
    /// Gets or sets the collection of refresh tokens associated with the user.
    /// </summary>
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}