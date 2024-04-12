using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INTEX.Models.DatabaseModels
{
    public class ProductRecommendation
    {
        [Key]
        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [ForeignKey("Recommendation1")]
        public int Recommendation1Id { get; set; }
        public Product Recommendation1 { get; set; }

        [ForeignKey("Recommendation2")]
        public int Recommendation2Id { get; set; }
        public Product Recommendation2 { get; set; }

        [ForeignKey("Recommendation3")]
        public int Recommendation3Id { get; set; }
        public Product Recommendation3 { get; set; }

        [ForeignKey("Recommendation4")]
        public int Recommendation4Id { get; set; }
        public Product Recommendation4 { get; set; }

        [ForeignKey("Recommendation5")]
        public int Recommendation5Id { get; set; }
        public Product Recommendation5 { get; set; }
    }
}
