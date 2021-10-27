using System;
using System.Collections.Generic;

namespace StoreApp.Models
{
    public partial class ProductType
    {
        public ProductType()
        {
            Products = new List<Product>();
        }

        public int TypeId { get; set; }
        public string Type { get; set; }

        public virtual List<Product> Products { get; set; }
    }
}
