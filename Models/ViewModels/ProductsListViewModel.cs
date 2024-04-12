using INTEX.Models.DatabaseModels;
using INTEX.Models.Infrastructure;
using System.Linq;

namespace INTEX.Models.ViewModels
{
    public class ProductsListViewModel
    {
        public IQueryable<Product> Products { get; set; }
        public PaginationInfo PaginationInfo { get; set; }
    }
}
