using INTEX.Models.DatabaseModels;
using INTEX.Models.Infrastructure;

namespace INTEX.Models.ViewModels
{
    public class OrdersListViewModel
    {
        public IQueryable<Order>? Orders { get; set; }
        public PaginationInfo PaginationInfo { get; set; }
    }
}
