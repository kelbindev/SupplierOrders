using Microsoft.AspNetCore.Mvc;
using SupplierOrders.Models;
using System.Diagnostics;

namespace SupplierOrders.Controllers;
public class HomeController : Controller
{

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult AccessDenied()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
