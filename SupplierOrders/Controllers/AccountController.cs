using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared;
using Shared.Constant;
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
    public IActionResult Logout()
    {
        Response.Cookies.Delete(CookiesName.UserName);

        Response.Cookies.Delete(CookiesName.JwtToken);

        Response.Cookies.Delete(CookiesName.RememberMe);

        return RedirectToAction("Index", "Home");
    }

    private void SetCookies(UserTokenDto token, bool rememberMe)
    {
        Response.Cookies.Append(CookiesName.UserName, token.UserName, new CookieOptions
        {
            HttpOnly = false,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.Now.AddDays(30)
        });

        Response.Cookies.Append(CookiesName.RememberMe, rememberMe.ToString() ,new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.Now.AddDays(30)
        });

        Response.Cookies.Append(CookiesName.JwtToken, token.JwtToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.Now.AddDays(30)
        });
    }
}
