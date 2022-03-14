using System;
using System.Collections.Generic;

namespace StoreApp.Models
{
    public partial class Order
    {
        public Order()
        {
            ProductInOrders = new List<ProductInOrder>();
        }

        public int OrderId { get; set; }
       
        public double TotalPrice { get; set; }
        public int BuyerId { get; set; }
        public DateTime Date { get; set; }

        public virtual Buyer Buyer { get; set; }
        
        public virtual List<ProductInOrder> ProductInOrders { get; set; }
    }
}
