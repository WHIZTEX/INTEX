namespace INTEX.Models.ViewModels;

public class DeleteConfirmationViewModel
{
    public Customer? Customer { get; set; }
    public Order? Order { get; set; }
    public Product? Product { get; set; }

    public DeleteConfirmationViewModel(Customer? customer, Order? order, Product? product)
    {
        Customer = customer;
        Order = order;
        Product = product;
    }
}