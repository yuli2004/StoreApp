using System;
using System.Collections.Generic;

namespace StoreApp.Models
{
    public partial class Buyer
    {
        public Buyer()
        {
            Orders = new HashSet<Order>();
            Reviews = new HashSet<Review>();
        }

        public int BuyerId { get; set; }
        public string Username { get; set; }

        public virtual User UsernameNavigation { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
