using INTEX.Models.DatabaseModels;
using INTEX.Models.Infrastructure;

namespace INTEX.Models.ViewModels
{
    public class CustomersListViewModel
    {
        public IQueryable<Customer> Customers { get; set;}
        public PaginationInfo PaginationInfo { get; set;}
        public Dictionary<string, List<string>>? Roles { get; set; } // Dictionary to store roles for each customer

    }
}
