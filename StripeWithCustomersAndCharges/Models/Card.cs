using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StripeWithCustomersAndCharges.Models
{
    public class Card
    {
        public string ID { get; set; }
        public string Line1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string CustomerID { get; set; }
        public string LastFour { get; set; }
        public string Brand { get; set; }

    }
}