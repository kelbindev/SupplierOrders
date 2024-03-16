using Contracts.Repository;
using Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Service.Contracts;
using Service.Utilities;
using Shared;
using Shared.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Service;
internal sealed class UserService(IUserRepository userRepository, IUserRefreshTokenRepository userRefreshTokenRepository, IOptions<AppSettings> appSettings)
    : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUserRefreshTokenRepository _userRefreshTokenRepository = userRefreshTokenRepository;
    private readonly IOptions<AppSettings> _appSettings = appSettings;

    public async Task<ApiResponse> Register(UserRegisterDto user)
    {
        if (await _userRepository.UserNameInUse(user.UserName))
            return ApiResponse.FailResponse("UserName already exists");

        if (await _userRepository.UserEmailInUse(user.UserEmail))
            return ApiResponse.FailResponse("UserEmail already exists");

        var randomSalt = Guid.NewGuid().ToString();
        var hashedPassowrd = await HMACHasher.HashValue(user.Password, randomSalt);

        var newUser = new User
        {
            UserName = user.UserName,
            UserEmail = user.UserEmail,
            Password = hashedPassowrd,
            PasswordSalt = randomSalt,
            CreatedBy = "Registeration",
            CreatedDate = DateOnly.FromDateTime(DateTime.UtcNow),
            UpdatedBy = "Registeration",
            UpdatedDate = DateOnly.FromDateTime(DateTime.UtcNow)
        };

        await _userRepository.Add(newUser);

        return ApiResponse.SuccessResponse(user);
    }

    public async Task<ApiResponse> Login(UserLoginDto userDto)
    {
        var user = await _userRepository.GetByUserName(userDto.UserName);

        if (user is null)
            return ApiResponse.FailResponse("User does not exists");

        var passwordSalt = user.PasswordSalt;
        var hashedPassword = await HMACHasher.HashValue(userDto.Password, passwordSalt);

        if (!hashedPassword.SequenceEqual(user.Password))
            return ApiResponse.FailResponse("Invalid password");

        var tokenResponse = await GenerateNewToken(user);

        return ApiResponse.SuccessResponse(tokenResponse);
    }

    public async Task<ApiResponse> RefreshToken(UserRefreshTokenDto user)
    {
        var usr = await _userRepository.GetByUserName(user.UserName);

        if (usr is null || usr.UserId == 0)
            return ApiResponse.FailResponse("User does not exists");

        var refreshToken = await _userRefreshTokenRepository.GetByUserIdAndRefreshToken(usr.UserId, user.refreshToken);

        if (refreshToken is null)
            return ApiResponse.FailResponse("Refresh Token does not exists");

        if (refreshToken.RefreshTokenExpired)
            return ApiResponse.FailResponse("Refresh Token expired");

        var tokenResponse = await GenerateNewToken(usr);

        return ApiResponse.SuccessResponse(tokenResponse);
    }

    private async Task<UserTokenDto> GenerateNewToken(User user)
    {
        var jwtToken = GenerateToken(user);
        var refreshToken = GenerateRefreshToken();

        var userRefreshToken = new UserRefreshToken
        {
            UserId = user.UserId,
            RefreshToken = refreshToken.RefreshTokenKey,
            RefreshTokenExpiry = refreshToken.RefreshTokenExpiry
        };

        if (await _userRefreshTokenRepository.Exists(userRefreshToken))
            await _userRefreshTokenRepository.Update(userRefreshToken);
        else
            await _userRefreshTokenRepository.Add(userRefreshToken);
        
        return new UserTokenDto(user.UserName, jwtToken.Token, refreshToken.RefreshTokenKey, refreshToken.RefreshTokenExpiry);
    }

    private JwtToken GenerateToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Value.JwtSettings.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim("UserId",user.UserId.ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Email, user.UserEmail)
        };

        var tokenExpiry = DateTime.Now.AddMinutes(Convert.ToInt32(_appSettings.Value.JwtSettings.AccessTokenExpiryInMinutes));

        var token = new JwtSecurityToken(
        issuer: _appSettings.Value.JwtSettings.Issuer,
            audience: _appSettings.Value.JwtSettings.Audience,
            claims: claims,
            expires: tokenExpiry,
            signingCredentials: credentials
            );

        string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

        return new JwtToken(jwtToken, tokenExpiry);
    }

    private RefreshToken GenerateRefreshToken()
    {
        int refreshTokenExpiryInMinutes = _appSettings.Value.JwtSettings.RefreshTokenExpiryInDays;

        string refreshTokenKey = Guid.NewGuid().ToString();
        DateTime refreshTokenExpiry = DateTime.Now.AddDays(Convert.ToInt32(refreshTokenExpiryInMinutes));

        return new RefreshToken(refreshTokenKey, refreshTokenExpiry);
    }
}
