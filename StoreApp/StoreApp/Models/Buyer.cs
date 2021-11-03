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

        public int Username { get; set; }
        public string Userid { get; set; }

        public virtual User User { get; set; }

        public virtual List<Order> Orders { get; set; }
        public virtual List<Review> Reviews { get; set; }
    }
}
