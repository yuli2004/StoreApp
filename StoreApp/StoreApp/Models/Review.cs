using System;
using System.Collections.Generic;

namespace StoreApp.Models
{
    public partial class Review
    {
        public int ProductId { get; set; }
        public string BuyerUsername { get; set; }
        public string Text { get; set; }
        public string SellerUsername { get; set; }
        public string Picture { get; set; }
        public int ReviewId { get; set; }

        public virtual Buyer BuyerUsernameNavigation { get; set; }
        public virtual Product Product { get; set; }
        public virtual Seller SellerUsernameNavigation { get; set; }
    }
}
