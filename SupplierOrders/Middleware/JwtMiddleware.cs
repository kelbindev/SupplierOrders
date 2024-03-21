using Microsoft.IdentityModel.Tokens;
using Shared.Constant;
using Shared.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SupplierOrders.Middleware;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            var token = context.Request.Cookies[CookiesName.JwtToken];
            var userName = context.Request.Cookies[CookiesName.UserName];
            var rememberMe = context.Request.Cookies[CookiesName.RememberMe] == "true";

            if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(userName))
            {
                await _next(context);
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero,
                ValidIssuer = _configuration["JwtSettings:Issuer"],
                ValidAudience = _configuration["JwtSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!))
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

            if (PrincipalExpiringSoon(principal) && rememberMe)
            {
                var newToken = GenerateToken(userName);

                if (newToken is null || string.IsNullOrWhiteSpace(newToken.Token)) { 
                    await _next(context);
                }

                context.Response.Cookies.Append(CookiesName.UserName, userName);

                context.Response.Cookies.Append(CookiesName.JwtToken, newToken.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.Now.AddDays(30)
                });

                context.Response.Cookies.Append(CookiesName.RememberMe, "true", new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.Now.AddDays(30)
                });

                context.Request.Headers.Add("Authorization", $"Bearer {newToken.Token}");
            }
            else
            {
                context.Request.Headers.Add("Authorization", $"Bearer {token}");
            }
        }
        catch
        {
            //ignore and client will receive unauthorized
        }

        await _next(context);
    }

    private JwtToken GenerateToken(string userName)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var tokenExpiry = DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:AccessTokenExpiryInMinutes"]));

        var token = new JwtSecurityToken(
        issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            expires: tokenExpiry,
            signingCredentials: credentials
            );

        string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

        return new JwtToken(jwtToken, tokenExpiry);
    }

    bool PrincipalExpiringSoon(ClaimsPrincipal principal)
    {
        var expClaim = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp);

        if (expClaim is null)
            return false;

        var expUnixSeconds = long.Parse(expClaim.Value);
        var expDateTime = DateTimeOffset.FromUnixTimeSeconds(expUnixSeconds).DateTime;

        var timeRemaining = expDateTime - DateTime.UtcNow;
        return timeRemaining < TimeSpan.FromMinutes(10);
    }
}
