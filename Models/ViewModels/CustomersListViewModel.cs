using INTEX.Models.DatabaseModels;
using INTEX.Models.Infrastructure;

namespace INTEX.Models.ViewModels
{
    public class CustomersListViewModel
    {
        public IQueryable<CustomersRolesListViewModel> CustomersRoles{ get; set;}
        public PaginationInfo PaginationInfo { get; set;}
    }
}
