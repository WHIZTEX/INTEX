using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INTEX.Models.DatabaseModels;

public class UserRecommendation
{
    [Key]
    [Required]
    [ForeignKey("Customer")]
    public string CustomerId { get; set; }
    public Customer Customer { get; set; }

    public int Recommendation1Id { get; set; }

    public int Recommendation2Id { get; set; }

    public int Recommendation3Id { get; set; }
}