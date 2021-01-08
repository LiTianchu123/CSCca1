using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StripeWeb.Models
{
    public class Transaction
    {
        public int ID { get; set; }
        public string CustomerID { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
