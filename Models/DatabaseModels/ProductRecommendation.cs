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

        public int Recommendation1Id { get; set; }

        public int Recommendation2Id { get; set; }

        public int Recommendation3Id { get; set; }

        public int Recommendation4Id { get; set; }

        public int Recommendation5Id { get; set; }
    }
}
