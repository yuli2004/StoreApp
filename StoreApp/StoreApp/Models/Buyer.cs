using System;
using System.Collections.Generic;

namespace StoreApp.Models
{
    public partial class Buyer
    {
        public Buyer()
        {
            Orders = new List<Order>();
        }

        public int BuyerId { get; set; }
        public string Username { get; set; }

        public virtual User UsernameNavigation { get; set; }
        public virtual List<Order> Orders { get; set; }
    }
}
