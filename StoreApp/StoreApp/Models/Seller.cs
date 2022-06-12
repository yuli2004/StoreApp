using System;
using System.Collections.Generic;

namespace StoreApp.Models
{
    public partial class Seller
    {
        public Seller()
        {
            Products = new List<Product>();
            
        }

        public int SellerId { get; set; }
        public string Username { get; set; }
        public string Picture { get; set; }
        public string Info { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public virtual User UsernameNavigation { get; set; }
        public virtual List<Product> Products { get; set; }
        

        //this method returns the profile picture url
        public string GetProfilePicture()
        {
            Random r = new Random();
            int i = r.Next(0, 100);
            if (Picture == "photogallery.png")
            {
                string pic = StoreApp.Services.StoreAPIProxy.GetImageURL() + "/" + SellerId + ".jpg"+ "?" + i;
                return pic;
            }
            else
                return Picture;
        }
    }
}
