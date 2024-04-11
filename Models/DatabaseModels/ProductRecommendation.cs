using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INTEX.Models.DatabaseModels
{
    public class ProductRecommendation
    {
        [Key]
        [ForeignKey("ProductRec")]
        public double product_ID { get; set; }
        //Product Rec is used for reference in the view
        public Product ProductRec { get; set; }



        [ForeignKey("Product1")]
        public string Recommendation_1 { get; set; }
        public Product Product1 { get; set; }


        [ForeignKey("Product2")]
        public string Recommendation_2 { get; set; }
        public Product Product2 { get; set; }


        [ForeignKey("Product3")]
        public string Recommendation_3 { get; set; }
        public Product Product3 { get; set; }


        [ForeignKey("Product4")]
        public string Recommendation_4 { get; set; }
        public Product Product4 { get; set; }


        [ForeignKey("Product5")]
        public string Recommendation_5 { get; set; }
        public Product Product5 { get; set; }
    }
}
