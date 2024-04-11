using Microsoft.AspNetCore.Mvc;
using INTEX.Models.DatabaseModels;
using INTEX.Models.Infrastructure;
using INTEX.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;

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
    public async Task<IActionResult> Customers()
    {
        CustomersListViewModel model = await _repo.GetCustomersListViewModel();
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
    public IActionResult CustomerForm(string? customerId, string? role)
    {
        Customer customer = _repo.GetCustomerById(customerId);
        List<string> roles = new List<string>();
        roles.Add(role);

        CustomersRolesListViewModel model = new CustomersRolesListViewModel(customer, roles);
        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> CustomerFormPost(CustomersRolesListViewModel model)
    {
        if (!ModelState.IsValid)
        {
            // If the model state is not valid, return to the form view with the model
            return View("CustomerForm", model);
        }

        await _repo.UpdateCustomerRole(model);
        _repo.UpdateCustomer(model.Customer);
        return RedirectToAction("Customers");
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

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public IActionResult DeleteProduct(Product product)
    {
        _repo.DeleteProduct(product);

        return RedirectToAction("Products");

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
