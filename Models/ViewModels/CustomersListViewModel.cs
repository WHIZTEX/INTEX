namespace INTEX.Models.ViewModels
{
    public class CustomersListViewModel
    {
        public IQueryable<Customer> Customers { get; set;}
        public PaginationInfo PaginationInfo { get; set;}
    }
}
