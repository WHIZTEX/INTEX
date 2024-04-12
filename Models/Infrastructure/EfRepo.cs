using INTEX.Models.DatabaseModels;
using INTEX.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using INTEX.Models.MachineLearning;
using Microsoft.AspNetCore.Identity;
using Microsoft.ML.OnnxRuntime;
using System.Security.Claims;

namespace INTEX.Models.Infrastructure;

public class EfRepo : IRepo
{
    private readonly ApplicationDbContext _context;
    private readonly InferenceSession _session;

    // ==== CONSTRUCTION ZONE BEGINS ====
    private readonly UserManager<Customer> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public EfRepo(ApplicationDbContext context, InferenceSession session, UserManager<Customer> userManager, RoleManager<IdentityRole> roleManager, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _session = session;
        _userManager = userManager;
        _roleManager = roleManager;
        _httpContextAccessor = httpContextAccessor;
    }

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
        var orders = _context.Orders
            .Where(o => !o.IsDeleted)
            .OrderByDescending(o => o.DateTime)
            .ToList(); // Materialize the query to a list

        var model = new OrdersListViewModel
        {
            Orders = orders.AsQueryable(),
            PaginationInfo = new PaginationInfo
            {
                CurrentPage = 0, // Set to the current page number
                ItemsPerPage = 20, // Set to the number of items per page
                TotalItems = orders.Count // Set to the total number of orders
            }
        };
        return model;
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

        var filteredProducts = products.ToList();

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
        // Implement logic to retrieve customer by ID
        return  _context.Customers
            .Include(c => c.HomeAddress)
            .First(c => c.Id == customerId);
    }

    public Customer GetCustomerByAspNetUserId(int aspNetUserId)
    {
        throw new NotImplementedException();
    }

    public Order GetOrderById(int? orderId)
    {
        if (orderId == null)
        {
            throw new ArgumentNullException(nameof(orderId));
        }

        return _context.Orders
            .Include(o => o.Transaction)
            .Include(o => o.Customer)
            .FirstOrDefault(o => o.Id == orderId);
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
        // Retrieve line items from cart
        var cart = _httpContextAccessor.HttpContext.Session.Get<List<LineItem>>("Cart") ?? new List<LineItem>();

        // ==== Define all the object variables that will need to go in the new order item ====

        // Update the customer object
        string customerId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        Customer customer = GetCustomerById(customerId);
        customer.FirstName = model.Order.Customer.FirstName; // checked, it gets passed in the model
        customer.LastName = model.Order.Customer.LastName; // checked, it gets passed in the model
        customer.Email = model.Order.Customer.Email; // checked, it gets passed in the model
        customer.PhoneNumber = model.Order.Customer.PhoneNumber; // checked, it gets passed in the model

        // Create billing address
        Address billingAddress = new Address
        {
            AddressLine1 = model.Transaction.BillingAddress.AddressLine1,
            AddressLine2 = model.Transaction.BillingAddress.AddressLine2,
            City = model.Transaction.BillingAddress.City,
            State = model.Transaction.BillingAddress.State,
            Code = model.Transaction.BillingAddress.Code,
            Country = model.Transaction.BillingAddress.Country
        };
        // Calculate transaction amount
            // subtotal
            decimal subtotal = 0;
            foreach (var lineItem in cart.ToList())
            {
                if (lineItem.Product != null)
                {
                    subtotal += lineItem.Quantity * lineItem.Product.Price;
                }
            }

            // tax
            decimal taxRate = 0.1637m; // Tax rate as 16.37%
            decimal tax = subtotal * taxRate;

            // total
            decimal total = subtotal + tax;

        // Create transaction
        Transaction transaction = new Transaction
        {
            AddressId = billingAddress.Id,
            BillingAddress = billingAddress,
            DateTime = new DateTime(),
            Amount = total,
            CardType = model.Transaction.CardType,
            EntryMode = model.Transaction.EntryMode,
            Bank = model.Transaction.Bank
        };

        // Create shipping address
        Address shippingAddress = new Address
        {
            AddressLine1 = model.Order.ShippingAddress.AddressLine1,
            AddressLine2 = model.Order.ShippingAddress.AddressLine2,
            City = model.Order.ShippingAddress.City,
            State = model.Order.ShippingAddress.State,
            Code = model.Order.ShippingAddress.Code,
            Country = model.Order.ShippingAddress.Country

        };

        // ==== Create the new order item based on the items that were previously defined =====
        Order order = new Order
        {
            CustomerId = customerId,
            Customer = customer,
            TransactionId = transaction.Id,
            Transaction = transaction,
            AddressId = shippingAddress.Id,
            ShippingAddress = shippingAddress,
            DateTime = new DateTime(),
            Type = "Online",
            IsFraud = false
        };

        ICollection<LineItem> lineItems = new List<LineItem>();

        foreach (LineItem lineItem in cart.ToList())
        {
            Product product = GetProductById(lineItem.ProductId);

            LineItem lineItemReady = new LineItem
            {
                OrderId = order.Id,
                Order = order,
                ProductId = lineItem.ProductId,
                Product = product,
                Quantity = lineItem.Quantity
            };

            lineItems.Add(lineItemReady);
        }

        order.LineItems = lineItems;

        // Run the fraud prediction
        var input = new FraudPredictionInput(order);
        var fraudPrediction = Convert.ToBoolean(
            _session.Run(new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("PredictionInput", input.AsTensor())
            })[0].AsTensor<long>().First());

        order.FraudPrediction = fraudPrediction;

        _context.Orders.Add(order);
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
                var existingAddress = _context.Addresses
                    .FirstOrDefault(a => a.Id == customer.AddressId);

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
        _context.Customers.Remove(customer);
        _context.SaveChanges();
    }

    public void DeleteOrder(Order order)
    {
        _context.Orders.Remove(order);
        _context.SaveChanges();
    }

    public void DeleteProduct(Product product)
    {
        product.IsDeleted = true;
        _context.Products.Update(product);
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

    public ProductRecommendationViewModel GenerateProductRecommendations(Product product)
    {
        ProductRecommendation recommendation = _context.ProductRecommendations
            .First(x => x.ProductId == product.Id);
        Product prod1 = _context.Products
            .First(x => x.Id == recommendation.Recommendation1Id);
        Product prod2 = _context.Products
            .First(x => x.Id == recommendation.Recommendation2Id);
        Product prod3 = _context.Products
            .First(x => x.Id == recommendation.Recommendation3Id);
        Product prod4 = _context.Products
            .First(x => x.Id == recommendation.Recommendation4Id);
        Product prod5 = _context.Products
            .First(x => x.Id == recommendation.Recommendation5Id);
        var model = new ProductRecommendationViewModel
        {
            Product = product,
            Recommendation1 = prod1,
            Recommendation2 = prod2,
            Recommendation3 = prod3,
            Recommendation4 = prod4,
            Recommendation5 = prod5
        };
        return model;
    }

    public CustomerRecommendationViewModel GenerateCustomerRecommendations(Customer customer)
    {
        UserRecommendation? recommendation = _context.UserRecommendations
            .FirstOrDefault(x => x.CustomerId == customer.Id);
        if (recommendation is null)
        {
            return new CustomerRecommendationViewModel();
        }
        Product prod1 = _context.Products
            .First(x => x.Id == recommendation.Recommendation1Id);
        Product prod2 = _context.Products
            .First(x => x.Id == recommendation.Recommendation2Id);
        Product prod3 = _context.Products
            .First(x => x.Id == recommendation.Recommendation3Id);
        var model = new CustomerRecommendationViewModel
        {
            Recommendation1 = prod1,
            Recommendation2 = prod2,
            Recommendation3 = prod3
        };
        return model;
    }
}