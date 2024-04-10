using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace INTEX.Models.DatabaseModels
{
    public class Customer : IdentityUser
    {
        [ForeignKey("Address")]
        public int AddressId { get; set; }
        public virtual Address HomeAddress { get; set; }
        
        [Required(ErrorMessage = "First Name is a required field")]
        [StringLength(64, ErrorMessage = "First Name must be no more than 64 characters long")]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "Last Name is a required field")]
        [StringLength(64, ErrorMessage = "Last Name must be no more than 64 characters long")]
        public string LastName { get; set; }
        
        [Required(ErrorMessage = "Birth Date is a required field")]
        public DateOnly BirthDate { get; set; }
        
        [Required(ErrorMessage = "Gender is a required field")]
        [StringLength(1, ErrorMessage = "Gender must be no more than 1 character long")]
        [RegularExpression("^[MFO]$", ErrorMessage = "Gender must be M(ale), F(emale), or O(ther)")]
        public string Gender { get; set; }
        
        public ICollection<Order> Orders { get; set; }
        
        public ICollection<Rating> Ratings { get; set; }
    }
}
