using Shared.User;
using Shared;

namespace Service.Contracts;
public interface IUserService
{
    Task<ApiResponse> Register(UserRegisterDto user);
    Task<(ApiResponse, UserTokenDto)> Login(UserLoginDto userDto);
}
