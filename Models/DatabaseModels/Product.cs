using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INTEX.Models.DatabaseModels
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Name is a required field")]
        [StringLength(128, ErrorMessage = "Name must be no larger than 128 characters")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Release Year is a required field")]
        [Range(0, int.MaxValue, ErrorMessage = "Release Year must be no smaller than 0")]
        public int ReleaseYear { get; set; }
        
        [Required(ErrorMessage = "Pieces is a required field")]
        [Range(0, int.MaxValue, ErrorMessage = "Pieces must be no smaller than 0")]
        public int Pieces { get; set; }
        
        [Required(ErrorMessage = "Price is a required field")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be no smaller than 0")]
        public decimal Price { get; set; }
        
        [Required(ErrorMessage = "Image Link is a required field")]
        [StringLength(256, ErrorMessage = "Name must be no larger than 256 characters")]
        public string ImgLink { get; set; }
        
        [Required(ErrorMessage = "Primary Color is a required field")]
        [StringLength(16, ErrorMessage = "Name must be no larger than 16 characters")]
        public string PrimaryColor { get; set; }
        
        [Required(ErrorMessage = "Secondary Color is a required field")]
        [StringLength(16, ErrorMessage = "Name must be no larger than 16 characters")]
        public string SecondaryColor { get; set; }
        
        [Required(ErrorMessage = "Description is a required field")]
        [StringLength(4096, ErrorMessage = "Name must be no larger than 4096 characters")]
        public string Description { get; set; }
        
        [Required(ErrorMessage = "Category is a required field")]
        [StringLength(64, ErrorMessage = "Name must be no larger than 64 characters")]
        public string Category { get; set; }

        [Required(ErrorMessage = "IsDeleted is a required field")]
        public bool IsDeleted { get; set; } = false;
        
        public ICollection<LineItem>? LineItems { get; set; }
        
        public ICollection<Rating>? Ratings { get; set; }
    }
}
