namespace UserAccountService.Models.Configuration;

/// <summary>
/// Represents the configuration settings for JWT (JSON Web Token) authentication.
/// </summary>
public class JwtSettings
{
    /// <summary>
    /// Gets or sets the secret key used for signing JWTs.
    /// </summary>
    public required string Secret { get; set; }

    /// <summary>
    /// Gets or sets the issuer of the JWTs.
    /// </summary>
    public required string Issuer { get; set; }

    /// <summary>
    /// Gets or sets the audience for the JWTs.
    /// </summary>
    public required string Audience { get; set; }

    /// <summary>
    /// Gets or sets the expiry time for JWTs, in minutes.
    /// </summary>
    public int ExpiryMinutes { get; set; }

    /// <summary>
    /// Gets or sets the expiry time for refresh tokens, in days.
    /// </summary>
    public int RefreshTokenExpiryDays { get; set; }
}