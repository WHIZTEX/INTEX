using Microsoft.AspNetCore.Mvc;
using INTEX.Models;
using INTEX.Models.DatabaseModels;
using INTEX.Models.Infrastructure;
using INTEX.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;

namespace INTEX.Controllers;

public class AdminController : Controller
{
    private readonly IRepo _repo;

    public AdminController(IRepo repo)
    {
        _repo = repo;
    }

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public IActionResult Index()
    {
        return View();
    }
    
    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public IActionResult Products(ProductsFilter filter)
    {
        ProductsListViewModel model = _repo.GetProductsListViewModel(filter);
        return View(model);
    }

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public IActionResult Customers()
    {
        CustomersListViewModel model = _repo.GetCustomersListViewModel();
        return View(model);
    }

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public IActionResult Orders()
    {
        OrdersListViewModel model = _repo.GetOrdersListViewModel();
        return View(model);
    }

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public IActionResult ProductForm(int? productId)
    {
        Product model = _repo.GetProductById(productId);
        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public IActionResult ProductForm(Product product)
    {
        _repo.UpdateProduct(product);
        return RedirectToAction("Products");
    }

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public IActionResult CustomerForm(string? customerId)
    {
        Customer model = _repo.GetCustomerById(customerId);
        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public IActionResult CustomerForm(Customer customer)
    {
        if (ModelState.IsValid)
        {
            _repo.UpdateCustomer(customer);
            return RedirectToAction("Customers");
        }
        foreach (var entry in ModelState)
        {
            if (entry.Value.Errors.Any())
            {
                string propName = entry.Key;
                var errMsgs = entry.Value.Errors.Select(error => error.ErrorMessage);
                foreach (var msg in errMsgs)
                {
                    Debug.WriteLine($"Validation error for property '{propName}': {msg}");
                }
            }
        }
        return Redirect(Request.Headers["Referer"].ToString());
    }

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public IActionResult OrderForm(int? orderId)
    {
        Order model = _repo.GetOrderById(orderId);
        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public IActionResult OrderForm(Order order)
    {
        _repo.UpdateOrder(order);
        return RedirectToAction("Orders");
    }

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public IActionResult DeleteConfirmation(Customer? customer, Product? product, Order? order)
    {
        DeleteConfirmationViewModel model;
        if (customer != null)
        {
            model = new DeleteConfirmationViewModel(customer, null, null);
            return View(model);
        }
        else if (order != null)
        {
            model = new DeleteConfirmationViewModel(null, order, null);
            return View(model);
        }
        else if (product != null)
        {
            model = new DeleteConfirmationViewModel(null, null, product);
            return View(model);
        }
        else
        {
            return View("Error");
        }
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public IActionResult Delete(object item)
    {
        switch (item)
        {
            case Customer customer:
            {
                _repo.DeleteCustomer(customer);
                return RedirectToAction("Customers");
            }
            case Order order:
            {
                _repo.DeleteOrder(order);
                return RedirectToAction("Orders");
            }
            case Product product:
            {
                _repo.DeleteProduct(product);
                return RedirectToAction("Products");
            }
            default:
            {
                return View("Error", item);
            }
        }
    }
}
