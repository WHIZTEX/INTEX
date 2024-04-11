using INTEX.Models;
using INTEX.Models.DatabaseModels;
using INTEX.Models.Infrastructure;
using INTEX.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

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
    public IActionResult Products(ProductsFilter filter)
    {
        ProductsListViewModel model = _repo.GetProductsListViewModel(filter);
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