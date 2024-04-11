using INTEX.Models.DatabaseModels;
using INTEX.Models.ViewModels;
using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

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
        var customers = _context.Customers
            .Where(p => p.IsDeleted == false)
            .Include(p => p.HomeAddress)
            .AsQueryable();

        var model = new CustomersListViewModel
        {
            Customers = customers,
            PaginationInfo = new PaginationInfo
            {
                CurrentPage = 0,
                ItemsPerPage = 20,
                TotalItems = customers.Count()
            }
        };
        return model;
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

    public Customer GetCustomerById(string? customerId)
    {
        if (customerId == null)
        {
            // Return a new instance of Customer
            return new Customer();
        }
        else
        {
            // Implement logic to retrieve product by ID
            return _context.Customers
                .Include(c => c.HomeAddress)
                .FirstOrDefault(c => c.Id == customerId);
        }
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
        if (productId == null)
        {
            // Return a new instance of Product
            return new Product();
        }
        else
        {
            // Implement logic to retrieve product by ID
            // For example:
            return _context.Products.FirstOrDefault(p => p.Id == productId);
        }
    }

    public Order ConfirmOrder(ConfirmOrderViewModel model)
    {
        throw new NotImplementedException();
    }

    public void UpdateCustomer(Customer customer)
    {
        _context.Customers.Update(customer);
        _context.SaveChanges();
    }

    public void UpdateOrder(Order order)
    {
        throw new NotImplementedException();
    }

    public void UpdateProduct(Product product)
    {
        if (product == null)
        {
            throw new ArgumentNullException(nameof(product));
        }

        var existingProduct = _context.Products.Find(product.Id);

        if (existingProduct != null)
        {
            // Detach the existing product from the DbContext
            _context.Entry(existingProduct).State = EntityState.Detached;

            // Update the existing product
            _context.Products.Update(product);
        }
        else
        {
            // Add the new product
            _context.Products.Add(product);
        }

        _context.SaveChanges();
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
        if (product == null)
        {
            throw new ArgumentNullException(nameof(product));
        }

        var existingProduct = _context.Products.Find(product.Id);

        if (existingProduct != null)
        {
            // Detach the existing product from the DbContext
            _context.Entry(existingProduct).State = EntityState.Detached;

            // Mark the existing product as deleted
            existingProduct.IsDeleted = true;

            // Update the existing product
            _context.Products.Update(product);
        }
        else
        {
            throw new ArgumentNullException(nameof(product));
        }

        _context.SaveChanges();
    }
}