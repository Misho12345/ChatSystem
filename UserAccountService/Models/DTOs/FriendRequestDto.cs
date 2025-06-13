namespace UserAccountService.Models.DTOs;

public class FriendRequestDto
{
    public Guid Id { get; set; }

    public Guid? RequesterId { get; set; }
    public string? RequesterName { get; set; }
    public string? RequesterTag { get; set; }

    public Guid? AddresseeId { get; set; }
    public string? AddresseeName { get; set; }
    public string? AddresseeTag { get; set; }

    public DateTime RequestedAt { get; set; }
}