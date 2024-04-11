using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INTEX.Models.DatabaseModels
{
    public class ProductRecommendation
    {
        [Key]
        [ForeignKey("ProductRec")]
        public int? ProductId { get; set; }
        public Product ProductRec { get; set; }

        [ForeignKey("Product1")]
        public int? Recommendation1Id { get; set; }
        public Product Product1 { get; set; }

        [ForeignKey("Product2")]
        public int? Recommendation2Id { get; set; }
        public Product Product2 { get; set; }

        [ForeignKey("Product3")]
        public int? Recommendation3Id { get; set; }
        public Product Product3 { get; set; }

        [ForeignKey("Product4")]
        public int? Recommendation4Id { get; set; }
        public Product Product4 { get; set; }

        [ForeignKey("Product5")]
        public int? Recommendation5Id { get; set; }
        public Product Product5 { get; set; }
    }
}
