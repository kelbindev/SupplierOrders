namespace Shared.User;

public class UserTokenDto
{
    public string UserName { get; set; }
    public string JwtToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiry { get; set; }

    public UserTokenDto(string userName, string jwtToken, string refreshToken, DateTime refreshTokenExpiry)
    {
        UserName = userName;
        JwtToken = jwtToken;
        RefreshToken = refreshToken;
        RefreshTokenExpiry = refreshTokenExpiry;
    }
}