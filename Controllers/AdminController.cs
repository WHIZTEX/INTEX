using Microsoft.AspNetCore.Mvc;
using INTEX.Models;
using INTEX.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

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
    public IActionResult Products()
    {
        ProductsListViewModel model = _repo.GetProductsListViewModel();
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
        return View();
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
    public IActionResult CustomerForm(int? customerId)
    {
        Customer model = _repo.GetCustomerById(customerId);
        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = "Administrator")]
    public IActionResult CustomerForm(Customer customer)
    {
        _repo.UpdateCustomer(customer);
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

    [HttpGet]
    [Authorize(Roles = "Administrator")]
    public IActionResult DeleteConfirmation(object item)
    {
        DeleteConfirmationViewModel model;
        switch (item)
        {
            case Customer customer:
                model = new DeleteConfirmationViewModel(customer, null, null);
                return View(model);
            case Order order:
                model = new DeleteConfirmationViewModel(null, order, null);
                return View(model);
            case Product product:
                model = new DeleteConfirmationViewModel(null, null, product);
                return View(model);
            default:
                return View("Error", item);
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