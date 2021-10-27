using System;
using System.Collections.Generic;

namespace StoreApp.Models
{
    public partial class Seller
    {
        public Seller()
        {
            Products = new List<Product>();
            Reviews = new List<Review>();
        }

        public string Username { get; set; }
        public string Picture { get; set; }
        public string Info { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string Country { get; set; }

        public virtual List<Product> Products { get; set; }
        public virtual List<Review> Reviews { get; set; }
    }
}
