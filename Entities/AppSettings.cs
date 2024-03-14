namespace Entities;
public record JwtSettings
{
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public int AccessTokenExpiryInMinutes { get; init; }
    public string Key { get; init; } = string.Empty;
    public int RefreshTokenExpiryInDays { get; init; }
}

public class AppSettings
{
    public JwtSettings JwtSettings { get; init; } = new();
}