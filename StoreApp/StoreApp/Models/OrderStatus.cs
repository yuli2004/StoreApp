using System;
using System.Collections.Generic;

namespace StoreApp.Models
{
    public partial class OrderStatus
    {
        public OrderStatus()
        {
            Orders = new List<Order>();
        }

        public int StatusId { get; set; }
        public string Status { get; set; }

        public virtual List<Order> Orders { get; set; }
    }
}
