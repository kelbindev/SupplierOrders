using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared;
using Shared.User;

namespace SupplierOrders.Controllers;
public class AccountController(IServiceManager services) : Controller
{
    private readonly IServiceManager _services = services;
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(UserLoginDto user)
    {
        var response = await _services.User.Login(user);

        if (response.Item1.Success)
        {
            SetCookies(response.Item2, user.RememberMe);
        }

        return Ok(response.Item1);
    }
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(UserRegisterDto user)
    {
        var response = await _services.User.Register(user);

        return Ok(response);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RefreshToken()
    {
        var userName = Request.Cookies["user_name"];
        var refreshToken = Request.Cookies["refresh_token"];

        if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(refreshToken))
            return Ok(ApiResponse.FailResponse("Session Expired"));

        var user = new UserRefreshTokenDto(userName, refreshToken);

        var response = await _services.User.RefreshToken(user);

        if (response.Item1.Success)
        {
            SetCookies(response.Item2, true);
        }

        return Ok(response.Item1);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("user_name");

        Response.Cookies.Delete("jwt_token");

        Response.Cookies.Delete("refresh_token");

        return RedirectToAction("Index", "Home");
    }

    private void SetCookies(UserTokenDto token, bool rememberMe)
    {
        Response.Cookies.Append("user_name",token.UserName);

        Response.Cookies.Append("jwt_token", token.JwtToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict
        });
    }
}
