namespace UserAccountService.Models.DTOs;

public class FriendDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Tag { get; set; } = string.Empty;
    public Guid FriendshipId { get; set; }
    public DateTime BecameFriendsAt { get; set; }
}