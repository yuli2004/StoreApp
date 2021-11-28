using System;
using System.Collections.Generic;

namespace StoreApp.Models
{
    public partial class Seller
    {
        public Seller()
        {
            Products = new HashSet<Product>();
            Reviews = new HashSet<Review>();
        }

        public int SellerId { get; set; }
        public string Username { get; set; }
        public string Picture { get; set; }
        public string Info { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public virtual User UsernameNavigation { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
