using System;
using System.Collections.Generic;
using System.Text;

namespace StoreApp.Models
{
    public partial class Style
    {
        public Style()
        {
            Products = new HashSet<Product>();
        }

        public int StyleId { get; set; }
        public string Style1 { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
