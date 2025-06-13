﻿namespace ChatService.Models.Configuration;

public class JwtSettings
{
    public required string Secret { get; set; }
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public int ExpiryMinutes { get; set; }
    public int RefreshTokenExpiryDays { get; set; }
}