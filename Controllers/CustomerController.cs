using Microsoft.AspNetCore.Mvc;
using INTEX.Models.DatabaseModels;
using INTEX.Models.Infrastructure;
using INTEX.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace INTEX.Controllers;

public class CustomerController : Controller
{
    private readonly IRepo _repo;

    public CustomerController(IRepo repo)
    {
        _repo = repo;
    }

    [HttpPost]
    [Authorize(Roles = "Customer,Administrator")]
    public IActionResult Cart(IQueryable<LineItem> lineItems)
    {
        return RedirectToAction("ConfirmOrder", new { lineItems = lineItems });
    }

    [HttpGet]
    [Authorize(Roles = "Customer,Administrator")]
    public IActionResult ConfirmOrder(IQueryable<LineItem> lineItems)
    {
        return View(lineItems);
    }

    [HttpPost]
    [Authorize(Roles = "Customer,Administrator")]
    public IActionResult ConfirmOrder(ConfirmOrderViewModel model)
    {
        Order order = _repo.ConfirmOrder(model);
        return RedirectToAction("OrderConfirmation", new { orderId = order.Id });
        /*
        if (ModelState.IsValid)
        {
            Order order = _repo.ConfirmOrder(model);
            return RedirectToAction("OrderConfirmation", new { orderId = order.Id });
        }
        return Redirect(Request.Headers.Referer.ToString());
        */
    }

    [HttpGet]
    [Authorize(Roles = "Customer,Administrator")]
    public IActionResult OrderConfirmation(int orderId)
    {
        Order model = _repo.GetOrderById(orderId);
        return View(model);
    }
    
    [HttpPost]
    [Authorize(Roles = "Customer,Administrator")]
    public IActionResult CustomerInfo(Customer customer)
    {
        _repo.UpdateCustomer(customer);
        return RedirectToAction("Index", "Home");
    }
}