using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INTEX.Models.DatabaseModels;

public class Transaction
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [Required]
    [ForeignKey("Address")]
    public int AddressId { get; set; }
    public virtual Address BillingAddress { get; set; }
        
    [Required(ErrorMessage = "Date Time is a required field")]
    public DateTime DateTime { get; set; }
    
    [Required(ErrorMessage = "Amount is a required field")]
    public decimal Amount { get; set; }
    
    [Required(ErrorMessage = "Card Type is a required field")]
    [StringLength(64, ErrorMessage = "Card Type must be no longer than 64 characters")]
    public string CardType { get; set; }
    
    [Required(ErrorMessage = "Entry Mode is a required field")]
    [StringLength(8, ErrorMessage = "Entry Mode must be no longer than 8 characters")]
    public string EntryMode { get; set; }
    
    [Required(ErrorMessage = "Bank is a required field")]
    [StringLength(64, ErrorMessage = "Bank must be no longer than 64 characters")]
    public string Bank { get; set; }

    [Required(ErrorMessage = "IsDeleted is a required field")]
    public bool IsDeleted { get; set; } = false;
}