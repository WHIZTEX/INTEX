using INTEX.Models;
using INTEX.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace INTEX.Controllers;

public class HomeController: Controller
{
    private readonly IRepo _repo;

    public HomeController(IRepo repo)
    {
        _repo = repo;
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
    
    [HttpGet]
    public IActionResult Products()
    {
        ProductsListViewModel model = _repo.GetProductsViewModel();
        return View(model);
    }

    [HttpGet]
    public IActionResult ProductDetails(int productId)
    {
        Product model = _repo.GetProductById(productId);
        return View(model);
    }
    
    [HttpGet]
    public IActionResult Cart()
    {
        return View();
    }

    [HttpGet]
    public IActionResult AboutUs()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Privacy()
    {
        return View();
    }
}