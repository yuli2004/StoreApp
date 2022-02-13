using System;
using System.Collections.Generic;

namespace StoreApp.Models
{
    public partial class PaintMaterial
    {
        public PaintMaterial()
        {
            Products = new List<Product>();
        }

        public int PMaterialId { get; set; }
        public string PMaterial { get; set; }

        public virtual List<Product> Products { get; set; }
    }
}
