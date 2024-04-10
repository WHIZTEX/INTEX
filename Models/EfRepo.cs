using Microsoft.CodeAnalysis;

namespace INTEX.Models;

public class EfRepo : IRepo
{
    
    private readonly ApplicationDbContext _context;


    public EfRepo(ApplicationDbContext context)
    {
        _context = context;
    }


    public IQueryable<AspNetRole> AspNetRoles => _context.AspNetRoles;


    // make this part an IQuerable to pull from product recommendation
    public IQueryable<ProductRecommendation> ProductRecommendations(int product_ID) => _context.ProductsRecommendations
                                                                        .Where(x => x.ProductId == product_ID)
                                                                        .Include(x => x.Recommendation1)
                                                                        .Include(x => x.Recommendation2)
                                                                        .Include(x => x.Recommendation3)
                                                                        .Include(x => x.Recommendation4)
                                                                        .Include(x => x.Recommendation5);

}