using INTEX.Models.ViewModels;

namespace INTEX.Models;

public interface IRepo
{
    public CustomersListViewModel GetCustomersListViewModel();
    public OrdersListViewModel GetOrdersListViewModel();
    public ProductsListViewModel GetProductsListViewModel();
    public Customer GetCustomerById(int? customerId);
    public Customer GetCustomerByAspNetUserId(int aspNetUserId);
    public Order GetOrderById(int? orderId);
    public Product GetProductById(int? productId);
    public Order ConfirmOrder(ConfirmOrderViewModel model);
    public void UpdateCustomer(Customer customer);
    public void UpdateOrder(Order order);
    public void UpdateProduct(Product product);
    public void DeleteCustomer(Customer customer);
    public void DeleteOrder(Order order);
    public void DeleteProduct(Product product);
}