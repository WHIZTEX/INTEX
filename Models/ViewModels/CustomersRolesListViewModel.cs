using INTEX.Models.DatabaseModels;
using INTEX.Models.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace INTEX.Models.ViewModels
{
    public class CustomersRolesListViewModel
    {
        public Customer Customer { get; set; }
        public List<string> Roles { get; set; }

        public CustomersRolesListViewModel(Customer? customer, List<string>? roles)
        {
            Customer = customer;
            Roles = roles;
        }

        public CustomersRolesListViewModel()
        {
        }
    }
}
