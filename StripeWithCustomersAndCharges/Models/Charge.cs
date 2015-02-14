using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StripeWithCustomersAndCharges.Models
{
    public class Charge
    {
        public int ID { get; set; }
        public string Amount { get; set; }

    }
}