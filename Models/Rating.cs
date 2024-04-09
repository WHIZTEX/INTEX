using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INTEX.Models;

public class Rating
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [ForeignKey("Customer")]
    public int CustomerId { get; set; }
    public virtual Customer Customer { get; set; }
    
    [Required]
    [ForeignKey("Product")]
    public int ProductId { get; set; }
    public virtual Product Product { get; set; }
    
    [Required(ErrorMessage = "Score is a required field")]
    [Range(1, 5, ErrorMessage = "Score must be between 1 and 5")]
    public int Score { get; set; }
}