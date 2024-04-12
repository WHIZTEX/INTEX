using System;

namespace INTEX.Models.Infrastructure
{
    public class ProductsFilter
    {
        public string[] Category { get; set; } = Array.Empty<string>();
        public string[] PrimaryColor { get; set; } = Array.Empty<string>();
        public string[] SecondaryColor { get; set; } = Array.Empty<string>();
        public int ProductsPerPage { get; set; } = 20; // Adjust as needed
    }
}
