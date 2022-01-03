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

        public int SellerId { get; set; }
        public string Username { get; set; }
        public string Picture { get; set; }
        public string Info { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public virtual User UsernameNavigation { get; set; }
        public virtual List<Product> Products { get; set; }
        public virtual List<Review> Reviews { get; set; }

        //this method returns the profile picture url
        public string GetProfilePicture()
        {
            string pic = StoreApp.Services.StoreAPIProxy.GetImageURL() + "/" + SellerId + ".jpg";
            return pic;
        }
    }
}
