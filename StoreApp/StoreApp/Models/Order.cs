using System;
using System.Collections.Generic;

namespace StoreApp.Models
{
    public partial class Order
    {
        public Order()
        {
            ProductInOrders = new HashSet<ProductInOrder>();
        }

        public int OrderId { get; set; }
        public int StatusId { get; set; }
        public double TotalPrice { get; set; }
        public int BuyerId { get; set; }
        public DateTime Date { get; set; }

        public virtual Buyer Buyer { get; set; }
        public virtual OrderStatus Status { get; set; }
        public virtual ICollection<ProductInOrder> ProductInOrders { get; set; }
    }
}
