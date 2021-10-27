using System;
using System.Collections.Generic;

namespace StoreApp.Models
{
    public partial class Buyer
    {
        public Buyer()
        {
            Orders = new List<Order>();
            Reviews = new List<Review>();
        }

        public string Username { get; set; }

        public virtual List<Order> Orders { get; set; }
        public virtual List<Review> Reviews { get; set; }
    }
}
