using INTEX.Models.DatabaseModels;
using INTEX.Models.ViewModels;
using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using INTEX.Models.MachineLearning;
using INTEX.Models.MachineLearning;
using Microsoft.AspNetCore.Identity;
using Microsoft.ML.OnnxRuntime;

namespace INTEX.Models.Infrastructure;

public class EfRepo : IRepo
{
    private readonly ApplicationDbContext _context;
    private readonly InferenceSession _session;
    public EfRepo(ApplicationDbContext context, InferenceSession session)
    {
        _context = context;
        _session = session;
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
        var input = new FraudPredictionInput(model);
        var fraudPrediction = Convert.ToBoolean(
            _session.Run(new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("PredictionInput", input.AsTensor())
            })[0].AsTensor<long>().First());
        var order = model.LineItems.First().Order!;
        order.FraudPrediction = fraudPrediction;
        _context.Orders.Update(order);
        _context.SaveChanges();
        return order;
    }

    public void UpdateCustomer(Customer customer)
    {
        if (customer == null)
        {
            throw new ArgumentNullException(nameof(customer));
        }

        // Check if the customer exists in the database
        var existingCustomer = _context.Customers.Find(customer.Id);

        if (existingCustomer != null)
        {
            // Check if the customer's address is being updated
            if (customer.HomeAddress != null)
            {
                // Check if the address already exists in the database
                var existingAddress = _context.Addresses.FirstOrDefault(a =>
                    a.AddressLine1 == customer.HomeAddress.AddressLine1 &&
                    a.AddressLine2 == customer.HomeAddress.AddressLine2 &&
                    a.City == customer.HomeAddress.City &&
                    a.State == customer.HomeAddress.State &&
                    a.Country == customer.HomeAddress.Country);

                if (existingAddress != null)
                {
                    // Associate the customer with the existing address
                    customer.HomeAddress = existingAddress;
                }
                else
                {
                    // Add the new address to the database
                    _context.Addresses.Add(customer.HomeAddress);
                }
            }

            // Update the existing customer
            _context.Entry(existingCustomer).CurrentValues.SetValues(customer);
            _context.SaveChanges();
        }
        else
        {
            throw new Exception("Customer not found.");
        }

        //_context.Customers.Update(customer);
        //_context.SaveChanges();
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