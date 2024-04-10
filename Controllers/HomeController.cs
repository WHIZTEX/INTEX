using INTEX.Models;
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
    public IActionResult Products()
    {
        return View();
    }

    [HttpGet]
    public IActionResult ProductDetails(int productId)
    {
        return View();
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