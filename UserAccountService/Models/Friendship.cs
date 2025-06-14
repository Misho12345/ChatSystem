using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserAccountService.Models;

public enum FriendshipStatus
{
    Pending,
    Accepted,
    Declined,
    Blocked
}

public sealed class Friendship
{
    [Key] public Guid Id { get; set; }


    public Guid RequesterId { get; set; }

    public User? Requester { get; set; }


    public Guid AddresseeId { get; set; }
    [ForeignKey("AddresseeId")] public User? Addressee { get; set; }


    public FriendshipStatus Status { get; set; }

    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    public DateTime? RespondedAt { get; set; }
}