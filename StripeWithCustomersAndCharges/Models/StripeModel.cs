using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace StripeWithCustomersAndCharges.Models
{
    public class StripeModel
    {
        [Required]
        public string Token { get; set; }
     
        public double Amount { get; set; }

        public string Name { get; set; }
        public bool Result { get; set; }
    }
}