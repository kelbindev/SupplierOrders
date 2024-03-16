namespace Shared.User;

public class UserTokenDto(string UserName, string JwtToken, string RefreshToken, DateTime RefreshTokenExpiry);