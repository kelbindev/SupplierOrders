namespace Shared.User;
public record UserRegisterDto(string UserName, string UserEmail, string Password, string ConfirmPassword);
public record UserLoginDto(string UserName, string Password, bool RememberMe);
public record UserRefreshTokenDto(string userName, string refreshToken);