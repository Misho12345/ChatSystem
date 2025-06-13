namespace UserAccountService.Models.DTOs;

public class FriendshipResultDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public Guid? FriendshipId { get; set; }
    public FriendshipStatus? Status { get; set; }
}