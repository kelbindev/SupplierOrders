namespace Shared.User;
public record JwtToken(string Token, DateTime TokenExpiry);

public record RefreshToken(string RefreshTokenKey, DateTime RefreshTokenExpiry);