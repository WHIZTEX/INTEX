using INTEX.Models.DatabaseModels;
using INTEX.Models.Infrastructure;

namespace INTEX.Models.ViewModels
{
    public class OrdersListViewModel
    {
        public IQueryable<Order>? Orders { get; set; }
        public required PaginationInfo PaginationInfo { get; set; }
        public bool? FraudPrediction { get; set; } // Add the FraudPrediction property here
    }
}
