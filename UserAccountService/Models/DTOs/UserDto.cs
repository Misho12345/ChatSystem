namespace UserAccountService.Models.DTOs;

/// <summary>
/// Represents a user entity in the system.
/// </summary>
public class UserDto
{
    /// <summary>
    /// The unique identifier of the user.
    /// </summary>
    /// <example>3f2504e0-4f89-11d3-9a0c-0305e82c3301</example>
    public Guid Id { get; set; }

    /// <summary>
    /// The name of the user.
    /// </summary>
    /// <remarks>
    /// This field contains the display name of the user.
    /// </remarks>
    /// <example>John Doe</example>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The tag of the user, in the format 'username#1234'.
    /// </summary>
    /// <remarks>
    /// The tag uniquely identifies the user within the system.
    /// </remarks>
    /// <example>john.doe#1234</example>
    public string Tag { get; set; } = string.Empty;

    /// <summary>
    /// The email address of the user.
    /// </summary>
    /// <remarks>
    /// The email must be a valid email address format.
    /// </remarks>
    /// <example>john.doe@example.com</example>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The date and time when the user was created.
    /// </summary>
    /// <example>2023-10-01T12:34:56Z</example>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserDto"/> class using a <see cref="User"/> entity.
    /// </summary>
    /// <param name="user">The user entity to map to the DTO.</param>
    public UserDto(User user)
    {
        Id = user.Id;
        Name = user.Name;
        Tag = user.Tag;
        Email = user.Email;
        CreatedAt = user.CreatedAt;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UserDto"/> class.
    /// </summary>
    public UserDto()
    {
    }
}