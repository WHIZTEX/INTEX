using INTEX.Models.DatabaseModels;

namespace INTEX.Models.ViewModels;

public class ProductRecommendationViewModel
{
    public Product Product { get; set; }
    
    public Product Recommendation1 { get; set; }
    
    public Product Recommendation2 { get; set; }
    
    public Product Recommendation3 { get; set; }
    
    public Product Recommendation4 { get; set; }
    
    public Product Recommendation5 { get; set; }
}