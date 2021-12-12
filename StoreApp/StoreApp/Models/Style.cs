using System;
using System.Collections.Generic;
using System.Text;

namespace StoreApp.Models
{
    public partial class Style
    {
        public Style()
        {
            Products = new List<Product>();
        }

        public int StyleId { get; set; }
        public string Style1 { get; set; }

        public virtual List<Product> Products { get; set; }
    }
}
