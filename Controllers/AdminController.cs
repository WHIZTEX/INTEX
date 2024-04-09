using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using INTEX.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using INTEX.Models.ViewModels;

namespace INTEX.Controllers;

public class AdminController : Controller
{

    private IProductRepository _repo;
    public AdminController(IProductRepository temp)
    {
        _repo = temp;
    }

    public IActionResult AdminProducts()
    {
        int pageSize = 50;
        var productsViewModel = new ProductsListViewModel
        {
            Products = _repo.Products
        }

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