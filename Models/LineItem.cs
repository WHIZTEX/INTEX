using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INTEX.Models
{
    public class LineItem
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        
        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        
        [Required(ErrorMessage = "Quantity is a required field")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be no smaller than 1")]
        public int Quantity { get; set; }
    }
}
