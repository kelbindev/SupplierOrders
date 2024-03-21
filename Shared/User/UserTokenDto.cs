namespace Shared.User;

public class UserTokenDto
{
    public string UserName { get; set; }
    public string JwtToken { get; set; }
    public UserTokenDto(string userName, string jwtToken)
    {
        UserName = userName;
        JwtToken = jwtToken;
    }
}