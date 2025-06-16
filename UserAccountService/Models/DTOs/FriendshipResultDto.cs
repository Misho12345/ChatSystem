namespace UserAccountService.Models.DTOs;

/// <summary>
/// Represents the result of a friendship-related operation.
/// </summary>
public class FriendshipResultDto
{
    /// <summary>
    /// Indicates whether the operation was successful.
    /// </summary>
    /// <example>true</example>
    public bool Success { get; set; }

    /// <summary>
    /// A message providing additional information about the operation result.
    /// </summary>
    /// <remarks>
    /// This message can describe the reason for failure or provide context for success.
    /// </remarks>
    /// <example>Friendship request sent successfully.</example>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// The unique identifier of the friendship, if applicable.
    /// </summary>
    /// <remarks>
    /// This field is null if the operation does not involve a specific friendship entity.
    /// </remarks>
    /// <example>3f2504e0-4f89-11d3-9a0c-0305e82c3301</example>
    public Guid? FriendshipId { get; set; }

    /// <summary>
    /// The current status of the friendship, if applicable.
    /// </summary>
    /// <remarks>
    /// This field is null if the operation does not involve a status update.
    /// </remarks>
    /// <example>Pending</example>
    public FriendshipStatus? Status { get; set; }
}