using System;

namespace INTEX.Models
{
    public class Customer
    {
        public int customer_ID { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public DateTime birth_date { get; set; } // Might have to handle this once we deploy and connect to database
        public string country_of_residence { get; set; }
        public string gender { get; set; }
        public decimal age { get; set; } // Do we want to calculate this dinamically somehow?
    }
}
