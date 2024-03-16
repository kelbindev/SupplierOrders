using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
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
    public async Task<IActionResult> Login(UserLoginDto user)
    {
        var response = await _services.User.Login(user);

        return Ok(response);
    }
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Register(UserRegisterDto user)
    {
        var response = await _services.User.Register(user);

        return Ok(response);
    }
}
