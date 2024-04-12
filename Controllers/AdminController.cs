using Microsoft.AspNetCore.Mvc;
using INTEX.Models.DatabaseModels;
using INTEX.Models.Infrastructure;
using INTEX.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using Microsoft.AspNetCore.Identity;

namespace INTEX.Controllers
{
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
        public IActionResult Orders(int page = 1, bool? FraudPrediction = null)
        {
            var ordersViewModel = _repo.GetOrdersListViewModel();

            // Initialize ordersQuery with an empty collection to prevent null reference exceptions
            var ordersQuery = ordersViewModel.Orders ?? Enumerable.Empty<Order>();

            // Apply filter for fraudulent orders if specified
            if (FraudPrediction.HasValue)
            {
                ordersQuery = ordersQuery.Where(o => o.IsFraud == FraudPrediction.Value);
            }

            // Paginate the orders
            int pageSize = 10; // Adjust as needed
            var paginatedOrders = ordersQuery
                .OrderByDescending(o => o.IsFraud) // Order by fraud prediction first
                .ThenByDescending(o => o.DateTime) // Then order by date time
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var viewModel = new OrdersListViewModel
            {
                Orders = paginatedOrders.AsQueryable(),
                PaginationInfo = new PaginationInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = ordersQuery.Count()
                },
                FraudPrediction = FraudPrediction // Pass the filter value back to the view
            };

            return View(viewModel); // Return the view with the paginated orders
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

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult ConfirmDeleteOrder(int? orderId)
        {
            Order order = _repo.GetOrderById(orderId);
            return View("DeleteOrderConfirmation", order);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult DeleteOrder(Order order)
        {
            _repo.DeleteOrder(order);
            return RedirectToAction("Orders");
        }


        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult ConfirmDeleteProduct(int? productId)
        {
            Product product = _repo.GetProductById(productId);
            return View("DeleteProductConfirmation", product);
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
        public IActionResult ConfirmDeleteCustomer(string? customerId)
        {
            Customer customer = _repo.GetCustomerById(customerId);
            return View("DeleteCustomerConfirmation", customer);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult DeleteCustomer(Customer customerIdOnly)
        {
            string customerId = customerIdOnly.Id;
            Customer customer = _repo.GetCustomerById(customerId);
            _repo.DeleteCustomer(customer);
            return RedirectToAction("Customers");
        }
    }
}
