﻿using System;

namespace INTEX.Models
{
    public class Order
    {
        public int transaction_ID { get; set; }
        public int customer_ID { get; set;}
        public DateTime date { get; set; }
        public string day_of_week { get; set; }
        public TimeOnly time { get; set; }
        public string type_of_card { get; set; }
        public string entry_mode { get; set; }
        public int amount { get; set; }
        public string type_of_transaction { get; set; }
        public string country_of_transaction { get; set; }
        public string shipping_address { get; set; }
        public string bank { get; set; }
        public bool fraud { get; set; }
    }
}
