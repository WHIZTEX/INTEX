﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INTEX.Models.DatabaseModels
{
    public class LineItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [ForeignKey("Order")]
        public int? OrderId { get; set; }
        public virtual Order? Order { get; set; }
        
        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        
        [Required(ErrorMessage = "Quantity is a required field")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be no smaller than 1")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "IsDeleted is a required field")]
        public bool IsDeleted { get; set; } = false;
    }
}
