using INTEX.Models.DatabaseModels;
using INTEX.Models.ViewModels;
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
        throw new NotImplementedException();
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
        _context.Products.Add(product);
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
        throw new NotImplementedException();
    }
}