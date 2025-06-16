using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserAccountService.Models;

/// <summary>
/// Represents the status of a friendship request.
/// </summary>
public enum FriendshipStatus
{
    /// <summary>
    /// Indicates that the friendship request is pending.
    /// </summary>
    Pending,

    /// <summary>
    /// Indicates that the friendship request has been accepted.
    /// </summary>
    Accepted,

    /// <summary>
    /// Indicates that the friendship request has been declined.
    /// </summary>
    Declined,

    /// <summary>
    /// Indicates that the friendship has been blocked.
    /// </summary>
    Blocked
}

/// <summary>
/// Represents a friendship between two users, including its status and timestamps.
/// </summary>
public sealed class Friendship
{
    /// <summary>
    /// Gets or sets the unique identifier for the friendship.
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user who initiated the friendship request.
    /// </summary>
    public Guid RequesterId { get; set; }

    /// <summary>
    /// Gets or sets the user who initiated the friendship request.
    /// </summary>
    public User? Requester { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user who received the friendship request.
    /// </summary>
    public Guid AddresseeId { get; set; }

    /// <summary>
    /// Gets or sets the user who received the friendship request.
    /// </summary>
    [ForeignKey("AddresseeId")]
    public User? Addressee { get; set; }

    /// <summary>
    /// Gets or sets the current status of the friendship.
    /// </summary>
    public FriendshipStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the friendship request was created.
    /// Defaults to the current UTC time.
    /// </summary>
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the date and time when the friendship request was responded to.
    /// Null if no response has been made.
    /// </summary>
    public DateTime? RespondedAt { get; set; }
}