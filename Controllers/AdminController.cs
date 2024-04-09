using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using INTEX.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace INTEX.Controllers;

public class AdminController : Controller
{
    private readonly IRepo _repo;

    public AdminController(IRepo repo)
    {
        _repo = repo;
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    

    public IActionResult Products()
    {
        return View();
    }

    public IActionResult Users()
    {
        return View();
    }

    public IActionResult Orders()
    {
        return View();
    }
}