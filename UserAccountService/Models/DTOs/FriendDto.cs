namespace UserAccountService.Models.DTOs;

/// <summary>
/// Represents a friend entity in the system.
/// </summary>
public class FriendDto
{
    /// <summary>
    /// The unique identifier of the friend.
    /// </summary>
    /// <example>3f2504e0-4f89-11d3-9a0c-0305e82c3301</example>
    public Guid Id { get; set; }

    /// <summary>
    /// The name of the friend.
    /// </summary>
    /// <remarks>
    /// This field contains the display name of the friend.
    /// </remarks>
    /// <example>John Doe</example>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The tag of the friend, in the format 'username#1234'.
    /// </summary>
    /// <remarks>
    /// The tag uniquely identifies the friend within the system.
    /// </remarks>
    /// <example>john.doe#1234</example>
    public string Tag { get; set; } = string.Empty;

    /// <summary>
    /// The unique identifier of the friendship associated with the friend.
    /// </summary>
    /// <example>3f2504e0-4f89-11d3-9a0c-0305e82c3301</example>
    public Guid FriendshipId { get; set; }

    /// <summary>
    /// The date and time when the friendship was established.
    /// </summary>
    /// <example>2023-10-01T12:34:56Z</example>
    public DateTime BecameFriendsAt { get; set; }
}