namespace Shared.User;
public class LoginResponseDto
{
    public string UserName { get; set; }

    public LoginResponseDto(string userName)
    {
        UserName = userName;
    }
}
