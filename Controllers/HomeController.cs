using System.Security.Claims;
using INTEX.Models.DatabaseModels;
using INTEX.Models.Infrastructure;
using INTEX.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var customer = _repo.GetCustomerById(userId);
        CustomerRecommendationViewModel model = _repo.GenerateCustomerRecommendations(customer);
        return View(model);
    }

    public IActionResult Products(int page = 1, string[] category = null, string[] color = null)
    {
        var filter = new ProductsFilter
        {
            Category = category ?? new string[0],
            PrimaryColor = color ?? new string[0],
            ProductsPerPage = 10 // Set as needed
        };

        var productsViewModel = _repo.GetProductsListViewModel(filter);

        // Paginate the products
        int pageSize = filter.ProductsPerPage;
        var paginatedProducts = productsViewModel.Products
            .OrderBy(p => p.Id) // Order by product ID (you can change this to any field you want)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var viewModel = new ProductsListViewModel
        {
            Products = paginatedProducts.AsQueryable(),
            PaginationInfo = new PaginationInfo
            {
                CurrentPage = page,
                ItemsPerPage = pageSize,
                TotalItems = productsViewModel.Products.Count() // Count of all filtered products
            }
        };

        return View(viewModel);
    }


    [HttpGet]
    public IActionResult ProductDetails(int productId)
    {
        Product product = _repo.GetProductById(productId);
        ProductRecommendationViewModel model = _repo.GenerateProductRecommendations(product);
        return View(model);
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

    [HttpGet]
    public IActionResult LiveChat()
    {
        return View();
    }
}
