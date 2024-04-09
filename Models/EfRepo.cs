using INTEX.Models.ViewModels;

namespace INTEX.Models;

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

    public ProductsListViewModel GetProductsListViewModel()
    {
        throw new NotImplementedException();
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