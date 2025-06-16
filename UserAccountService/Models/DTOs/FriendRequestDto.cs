namespace UserAccountService.Models.DTOs;

/// <summary>
/// Represents a request to send a friend request.
/// </summary>
public class FriendRequestDto
{
    /// <summary>
    /// The unique identifier of the friend request.
    /// </summary>
    /// <example>3f2504e0-4f89-11d3-9a0c-0305e82c3301</example>
    public Guid Id { get; set; }

    /// <summary>
    /// The unique identifier of the user sending the friend request.
    /// </summary>
    /// <remarks>
    /// This field is optional and may be null if the requester is not identified.
    /// </remarks>
    /// <example>3f2504e0-4f89-11d3-9a0c-0305e82c3301</example>
    public Guid? RequesterId { get; set; }

    /// <summary>
    /// The name of the user sending the friend request.
    /// </summary>
    /// <remarks>
    /// This field is optional and may be null if the requester name is not provided.
    /// </remarks>
    /// <example>john.doe</example>
    public string? RequesterName { get; set; }

    /// <summary>
    /// The tag of the user sending the friend request, in the format 'username#1234'.
    /// </summary>
    /// <remarks>
    /// This field is optional and may be null if the requester tag is not provided.
    /// </remarks>
    /// <example>john.doe#1234</example>
    public string? RequesterTag { get; set; }

    /// <summary>
    /// The unique identifier of the user receiving the friend request.
    /// </summary>
    /// <remarks>
    /// This field is optional and may be null if the addressee is not identified.
    /// </remarks>
    /// <example>3f2504e0-4f89-11d3-9a0c-0305e82c3301</example>
    public Guid? AddresseeId { get; set; }

    /// <summary>
    /// The name of the user receiving the friend request.
    /// </summary>
    /// <remarks>
    /// This field is optional and may be null if the addressee name is not provided.
    /// </remarks>
    /// <example>jane.doe</example>
    public string? AddresseeName { get; set; }

    /// <summary>
    /// The tag of the user receiving the friend request, in the format 'username#1234'.
    /// </summary>
    /// <remarks>
    /// This field is optional and may be null if the addressee tag is not provided.
    /// </remarks>
    /// <example>jane.doe#1234</example>
    public string? AddresseeTag { get; set; }

    /// <summary>
    /// The date and time when the friend request was sent.
    /// </summary>
    /// <example>2023-10-01T12:34:56Z</example>
    public DateTime RequestedAt { get; set; }
}