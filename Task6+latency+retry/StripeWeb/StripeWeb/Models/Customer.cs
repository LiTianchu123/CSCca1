using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StripeWeb.Models
{
    [Table("Customer")]
    public class Customer
    {
        public string CustomerID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Balance { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
