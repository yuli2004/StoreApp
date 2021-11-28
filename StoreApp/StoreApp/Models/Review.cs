using System;
using System.Collections.Generic;

namespace StoreApp.Models
{
    public partial class Review
    {
        public int ProductId { get; set; }
        public int BuyerId { get; set; }
        public string Text { get; set; }
        public int SellerId { get; set; }
        public string Picture { get; set; }
        public int ReviewId { get; set; }

        public virtual Buyer Buyer { get; set; }
        public virtual Product Product { get; set; }
        public virtual Seller Seller { get; set; }
    }
}
