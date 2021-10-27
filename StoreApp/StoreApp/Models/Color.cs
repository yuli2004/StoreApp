using System;
using System.Collections.Generic;

namespace StoreApp.Models
{
    public partial class Color
    {
        public Color()
        {
            Products = new List<Product>();
        }

        public int ColorId { get; set; }
        public string Color1 { get; set; }

        public virtual List<Product> Products { get; set; }
    }
}
