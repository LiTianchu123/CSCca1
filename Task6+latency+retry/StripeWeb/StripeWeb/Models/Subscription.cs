using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StripeWeb.Models
{
    public class Subscription
    {
        public string SubscriptionID { get; set; }
        public string CustomerID { get; set; }
        public int UnitAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


    }
}
