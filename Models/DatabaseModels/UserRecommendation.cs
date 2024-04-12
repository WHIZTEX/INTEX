using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INTEX.Models.DatabaseModels;

public class UserRecommendation
{
    [Key]
    [Required]
    [ForeignKey("Customer")]
    public int CustomerRecommendationId { get; set; }
    public Customer Customer { get; set; }

    [ForeignKey("Recommendation1")]
    public int Recommendation1Id { get; set; }
    public Product Recommendation1 { get; set; }

    [ForeignKey("Recommendation2")]
    public int Recommendation2Id { get; set; }
    public Product Recommendation2 { get; set; }

    [ForeignKey("Recommendation3")]
    public int Recommendation3Id { get; set; }
    public Product Recommendation3 { get; set; }
}