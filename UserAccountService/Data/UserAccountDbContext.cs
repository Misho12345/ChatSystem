using Microsoft.EntityFrameworkCore;
using UserAccountService.Models;

namespace UserAccountService.Data;

/// <summary>
/// Represents the database context for the User Account Service.
/// Provides access to the database tables and configuration for entity relationships.
/// </summary>
public class UserAccountDbContext(DbContextOptions<UserAccountDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Gets or sets the database table for users.
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// Gets or sets the database table for friendships.
    /// </summary>
    public DbSet<Friendship> Friendships { get; set; }

    /// <summary>
    /// Gets or sets the database table for refresh tokens.
    /// </summary>
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    /// <summary>
    /// Configures the entity relationships and indexes for the database tables.
    /// </summary>
    /// <param name="modelBuilder">The <see cref="ModelBuilder"/> used to configure the entity models.</param>
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