using INTEX.Models.DatabaseModels;
using INTEX.Models.ViewModels;

namespace INTEX.Models.Infrastructure;

public class EfRepo : IRepo
{
    private readonly ApplicationDbContext _context;
    public EfRepo(ApplicationDbContext context)
    {
        _context = context;
    }

    public CustomersListViewModel GetCustomersListViewModel()
    {
        throw new NotImplementedException();
    }

    public OrdersListViewModel GetOrdersListViewModel()
    {
        throw new NotImplementedException();
    }

    public ProductsListViewModel GetProductsListViewModel(ProductsFilter filter)
    {
        var products = _context.Products
            .Where(p => p.IsDeleted == false)
            .AsQueryable();
        if (filter.Category.Length > 0)
        {
            products = products
                .Where(p => filter.Category.Contains(p.Category))
                .AsQueryable();
        }
        if (filter.PrimaryColor.Length > 0)
        {
            products = products
                .Where(p => filter.PrimaryColor.Contains(p.PrimaryColor))
                .AsQueryable();
        }
        if (filter.SecondaryColor.Length > 0)
        {
            products = products
                .Where(p => filter.SecondaryColor.Contains(p.SecondaryColor))
                .AsQueryable();
        }

        var model = new ProductsListViewModel
        {
            Products = products,
            PaginationInfo = new PaginationInfo
            {
                CurrentPage = 0,
                ItemsPerPage = filter.ProductsPerPage,
                TotalItems = products.Count()
            }
        };
        return model;
    }

    public Customer GetCustomerById(int? customerId)
    {
        throw new NotImplementedException();
    }

    public Customer GetCustomerByAspNetUserId(int aspNetUserId)
    {
        throw new NotImplementedException();
    }

    public Order GetOrderById(int? orderId)
    {
        throw new NotImplementedException();
    }

    public Product GetProductById(int? productId)
    {
        throw new NotImplementedException();
    }

    public Order ConfirmOrder(ConfirmOrderViewModel model)
    {
        throw new NotImplementedException();
    }

    public void UpdateCustomer(Customer customer)
    {
        throw new NotImplementedException();
    }

    public void UpdateOrder(Order order)
    {
        throw new NotImplementedException();
    }

    public void UpdateProduct(Product product)
    {
        throw new NotImplementedException();
    }

    public void DeleteCustomer(Customer customer)
    {
        throw new NotImplementedException();
    }

    public void DeleteOrder(Order order)
    {
        throw new NotImplementedException();
    }

    public void DeleteProduct(Product product)
    {
        throw new NotImplementedException();
    }
}