﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INTEX.Models.DatabaseModels
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        [ForeignKey("Customers")]
        public string CustomerId { get; set;}
        public virtual Customer Customer { get; set; }
        
        [Required]
        [ForeignKey("Order")]
        public int TransactionId { get; set; }
        public virtual Transaction Transaction { get; set; }
        
        [Required]
        [ForeignKey("Address")]
        public int AddressId { get; set; }
        public virtual Address ShippingAddress { get; set; }
        
        [Required(ErrorMessage = "Date Time is a required field")]
        public DateTime DateTime { get; set; }
        
        [Required(ErrorMessage = "Type is a required field")]
        [StringLength(16, ErrorMessage = "Type must be no more than 16 characters")]
        public string Type { get; set; }
        
        [Required(ErrorMessage = "Fraud Prediction is a required field")]
        public bool? FraudPrediction { get; set; }
        
        [Required(ErrorMessage = "Is Fraud is a required field")]
        public bool? IsFraud { get; set; }

        [Required(ErrorMessage = "IsDeleted is a required field")]
        public bool IsDeleted { get; set; } = false;
        
        public ICollection<LineItem>? LineItems { get; set; }
    }
}
