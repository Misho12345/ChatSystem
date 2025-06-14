using System.ComponentModel.DataAnnotations;

namespace UserAccountService.Models;

public sealed class User
{
    [Key] public Guid Id { get; set; }


    [MaxLength(100)] public required string Name { get; set; }


    [MaxLength(50)] public required string Tag { get; set; }


    [MaxLength(255)] [EmailAddress] public required string Email { get; set; }


    [MaxLength(255)] public required string PasswordHash { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Friendship> Friendships1 { get; set; } = new List<Friendship>();
    public ICollection<Friendship> Friendships2 { get; set; } = new List<Friendship>();
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}