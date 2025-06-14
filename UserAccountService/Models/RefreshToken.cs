using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserAccountService.Models;

public sealed class RefreshToken
{
    [MaxLength(8192)][Key] public required string Token { get; set; }

    [MaxLength(255)] public required string JwtId { get; set; }


    public Guid UserId { get; set; }
    [ForeignKey("UserId")] public User? User { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpiryDate { get; set; }

    public bool Used { get; set; } = false;
    public bool Invalidated { get; set; } = false;
}