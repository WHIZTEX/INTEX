using INTEX.Models.DatabaseModels;
using INTEX.Models.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

public static class SessionExtensions
{
    public static void Set<T>(this ISession session, string key, T value)
    {
        session.SetString(key, JsonSerializer.Serialize(value));
    }

    public static T Get<T>(this ISession session, string key)
    {
        var value = session.GetString(key);
        return value == null ? default : JsonSerializer.Deserialize<T>(value);
    }
}

namespace INTEX.Controllers
{
    public class CartController : Controller
    {
        private readonly IRepo _repo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartController(IRepo repo, IHttpContextAccessor httpContextAccessor)
        {
            _repo = repo;
            _httpContextAccessor = httpContextAccessor;
        }

        // Action method for adding a product to the cart
        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            var product = _repo.GetProductById(productId);

            if (product == null)
            {
                return NotFound();
            }

            var cart = _httpContextAccessor.HttpContext.Session.Get<List<LineItem>>("Cart") ?? new List<LineItem>();

            // Check if the product is already in the cart
            var existingLineItem = cart.FirstOrDefault(item => item.ProductId == productId);

            if (existingLineItem != null)
            {
                // If the product is already in the cart, increase the quantity
                existingLineItem.Quantity++;
            }
            else
            {
                // If the product is not in the cart, add it as a new line item
                cart.Add(new LineItem { ProductId = productId, Quantity = 1 });
            }

            _httpContextAccessor.HttpContext.Session.Set("Cart", cart);

            return RedirectToAction("Products", "Home");
        }
    }
}
