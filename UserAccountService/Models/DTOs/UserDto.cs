namespace UserAccountService.Models.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Tag { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public UserDto(User user)
    {
        Id = user.Id;
        Name = user.Name;
        Tag = user.Tag;
        Email = user.Email;
        CreatedAt = user.CreatedAt;
    }

    public UserDto()
    {
    }
}