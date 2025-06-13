using Microsoft.EntityFrameworkCore;
using UserAccountService.Models;

namespace UserAccountService.Data;

public class UserAccountDbContext(DbContextOptions<UserAccountDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Friendship> Friendships { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(u => u.Tag).IsUnique();
            entity.HasIndex(u => u.Email).IsUnique();
            entity.HasIndex(u => u.Name);
        });

        modelBuilder.Entity<Friendship>(entity =>
        {
            entity.HasIndex(f => new { f.RequesterId, f.AddresseeId }).IsUnique();

            entity.HasOne(f => f.Requester)
                .WithMany(u => u.Friendships1)
                .HasForeignKey(f => f.RequesterId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(f => f.Addressee)
                .WithMany(u => u.Friendships2)
                .HasForeignKey(f => f.AddresseeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<RefreshToken>(entity => { entity.HasIndex(rt => rt.UserId); });
    }
}