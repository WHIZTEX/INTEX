using System.ComponentModel.DataAnnotations;

namespace INTEX.Models;

public class Address
{
    [Key]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Address Line 1 is a required field")]
    [StringLength(64, ErrorMessage = "Address Line 1 must be no more than 64 characters")]
    public string AddressLine1 { get; set; }
    
    [StringLength(64, ErrorMessage = "Address Line 2 must be no more than 64 characters")]
    public string? AddressLine2 { get; set; }
    
    
    [StringLength(64, ErrorMessage = "City must be no more than 64 characters")]
    public string? City { get; set; }
    
    [StringLength(64, ErrorMessage = "State must be no more than 64 characters")]
    public string? State { get; set; }
    
    [StringLength(64, ErrorMessage = "Code must be no more than 64 characters")]
    public string? Code { get; set; }
    
    [Required(ErrorMessage = "Country is a required field")]
    [StringLength(64, ErrorMessage = "Country must be no more than 64 characters")]
    public string Country { get; set; }
}