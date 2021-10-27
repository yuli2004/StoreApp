using System;
using System.Collections.Generic;

namespace StoreApp.Models
{
    public partial class Product
    {
        public Product()
        {
            ProductInOrders = new List<ProductInOrder>();
            Reviews = new List<Review>();
        }

        public string SellerUsername { get; set; }
        public string ProductName { get; set; }
        public string Details { get; set; }
        public int ProductId { get; set; }
        public int TypeId { get; set; }
        public double Price { get; set; }
        public string Material { get; set; }
        public int ColorId { get; set; }
        public bool IsActive { get; set; }
        public DateTime AdvertisingDate { get; set; }
        public string Picture { get; set; }
        public string ShipsFrom { get; set; }
        public int Stock { get; set; }

        public virtual Color Color { get; set; }
        public virtual Seller SellerUsernameNavigation { get; set; }
        public virtual ProductType Type { get; set; }
        public virtual List<ProductInOrder> ProductInOrders { get; set; }
        public virtual List<Review> Reviews { get; set; }
    }
}
