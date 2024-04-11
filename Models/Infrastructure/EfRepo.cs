using INTEX.Models.DatabaseModels;
using INTEX.Models.ViewModels;
using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using INTEX.Models.MachineLearning;
using Microsoft.AspNetCore.Identity;
using Microsoft.ML.OnnxRuntime;
using Microsoft.Extensions.Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace INTEX.Models.Infrastructure;

public class EfRepo : IRepo
{
    private readonly ApplicationDbContext _context;
    private readonly InferenceSession _session;

    // ==== CONSTRUCTION ZONE BEGINS ====
    private readonly UserManager<Customer> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public EfRepo(ApplicationDbContext context, InferenceSession session, UserManager<Customer> userManager, RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _session = session;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    /*
    public async Task<List<string>> GetRolesForCustomerAsync(string customerId)
    {
        var user = await _userManager.FindByIdAsync(customerId);
        if (user == null)
        {
            // Handle the case where user is not found
            return new List<string>();
        }

        var roles = await _userManager.GetRolesAsync(user);
        return roles.ToList();
    }
    */

    // ==== CONSTRUCTION ZONE ENDS ====

    public async Task<CustomersListViewModel> GetCustomersListViewModel()
    {
        var customers = _context.Customers
        .Where(p => p.IsDeleted == false)
        .Include(p => p.HomeAddress)
        .AsQueryable();

        var customerRolesList = new List<CustomersRolesListViewModel>();

        foreach (var customer in customers)
        {
            var roles = await _userManager.GetRolesAsync(customer);

            var customerRole = new CustomersRolesListViewModel(customer, roles.ToList());

            customerRolesList.Add(customerRole);
        }

        var model = new CustomersListViewModel
        {
            CustomersRoles = customerRolesList.AsQueryable(),
            PaginationInfo = new PaginationInfo
            {
                CurrentPage = 0,
                ItemsPerPage = 20,
                TotalItems = customerRolesList.Count()
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
            .Where(p => p.IsDeleted == false);

        if (filter.Category.Length > 0)
        {
            products = products
                .Where(p => filter.Category.Contains(p.Category));
        }

        if (filter.PrimaryColor.Length > 0)
        {
            products = products
                .Where(p => filter.PrimaryColor.Contains(p.PrimaryColor));
        }

        if (filter.SecondaryColor.Length > 0)
        {
            products = products
                .Where(p => filter.SecondaryColor.Contains(p.SecondaryColor));
        }

        var filteredProducts = products.ToList(); // Materialize the filtered query

        var model = new ProductsListViewModel
        {
            Products = filteredProducts.AsQueryable(),
            PaginationInfo = new PaginationInfo
            {
                CurrentPage = 0,
                ItemsPerPage = filter.ProductsPerPage,
                TotalItems = filteredProducts.Count()
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

    public async Task UpdateCustomerRole(CustomersRolesListViewModel model)
    {
        var customer = model.Customer;
        var newRole = model.Roles[0];

        // Get the user associated with the customer
        var user = await _userManager.FindByIdAsync(customer.Id);

        // Get the current roles of the user
        var currentRoles = await _userManager.GetRolesAsync(user);

        // Remove all existing roles
        await _userManager.RemoveFromRolesAsync(user, currentRoles);

        // Add the new role
        await _userManager.AddToRoleAsync(user, newRole);
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
        product.IsDeleted = true;
        _context.SaveChanges();
    }

    public void PermDeleteProduct(Product product)
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

    // addtion for the recommedation
    public IQueryable<ProductRecommendation> ProductRecommendations(int productId) => _context.ProductRecommendations
                                                                           .Where(x => x.ProductId == productId)
                                                                           .Include(x => x.Product1)
                                                                           .Include(x => x.Product2)
                                                                           .Include(x => x.Product3)
                                                                           .Include(x => x.Product4)
                                                                           .Include(x => x.Product5)
                                                                           .Include(x => x.ProductRec);
}