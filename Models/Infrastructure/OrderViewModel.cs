using INTEX.Models.DatabaseModels;

namespace INTEX.Models.Infrastructure
{
    internal class OrderViewModel
    {
        public int Id { get; set; }
        public required string CustomerId { get; set; }
        public DateTime DateTime { get; set; }
        public required Transaction Transaction { get; set; }
        public required string Type { get; set; }
        public bool? FraudPrediction { get; set; }
        public bool? IsFraud { get; set; }
    }
}