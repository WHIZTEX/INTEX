using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using INTEX.Models;
using Microsoft.Extensions.Logging;

namespace INTEX.Controllers;

public class AdminController : Controller
{
    private readonly ILogger<CustomerController> _logger;

    public AdminController(ILogger<CustomerController> logger)
    {
        _logger = logger;
    }

    public IActionResult AdminProducts()
    {
        return View();
    }

    public IActionResult AdminUsers()
    {
        return View();
    }

    public IActionResult ReviewOrders()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}