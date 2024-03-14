namespace Shared.User;

public class UserTokenDto(string userName, string jwtToken, string refreshToken, DateTime refreshTokenExpiry);