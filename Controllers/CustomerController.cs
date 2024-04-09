using Microsoft.AspNetCore.Mvc;
using INTEX.Models;
using Microsoft.AspNetCore.Authorization;

namespace INTEX.Controllers;

public class CustomerController : Controller
{
    private readonly IRepo _repo;

    public CustomerController(IRepo repo, ApplicationDbContext context)
    {
        _repo = repo;
    }

    [HttpPost]
    [Authorize(Roles = "Customer,Administrator")]
    public IActionResult Cart(LineItem[] lineItems)
    {
        Order order = new Order();
        return RedirectToAction("OrderConfirmation", new { orderId = order.Id });
    }

    [HttpGet]
    [Authorize(Roles = "Customer,Administrator")]
    public IActionResult OrderConfirmation(int orderId)
    {
        return View();
    }
    

    [HttpGet]
    [Authorize(Roles = "Customer,Administrator")]
    public IActionResult CustomerInfo()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Customer,Administrator")]
    public IActionResult CustomerInfo(Customer customer)
    {
        return RedirectToAction("Index", "Home");
    }
}