using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INTEX.Models.DatabaseModels
{
    public class ProductRecommendation
    {
        [Key]
        [ForeignKey("product_ID")]
        public string IfYouPurchased { get; set; }
        public Product product_ID { get; set; }



        [ForeignKey("Product1")]
        public string Recommedation_1 { get; set; }
        public Product Product1 { get; set; }


        [ForeignKey("Product2")]
        public string Recommedation_2 { get; set; }
        public Product Product2 { get; set; }


        [ForeignKey("Product3")]
        public string Recommedation_3 { get; set; }
        public Product Product3 { get; set; }


        [ForeignKey("Product4")]
        public string Recommedation_4 { get; set; }
        public Product Product4 { get; set; }


        [ForeignKey("Product5")]
        public string Recommedation_5 { get; set; }
        public Product Product5 { get; set; }
    }
}
