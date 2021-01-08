using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StripeWeb.Models
{
    public class PaymentMethod
    {
        public string PaymentMethodID { get; set; }
        public string CustomerID { get; set; }
        public string Brand { get; set; }
        public int Last4 { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


    }
}
