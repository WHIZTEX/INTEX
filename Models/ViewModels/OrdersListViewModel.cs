namespace INTEX.Models.ViewModels
{
    public class OrdersListViewModel
    {
        public IQueryable<Order> Orders { get; set; }
        public PaginationInfo PaginationInfo { get; set; }
    }
}
