using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StripeWithCustomersAndCharges.Models
{
    public class CustomerWithCard
    {
        public string ID { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }

    }
}

