using System;
using System.Collections.Generic;

namespace StoreApp.Models
{
    public partial class SurfaceMaterial
    {
        public SurfaceMaterial()
        {
            Products = new List<Product>();
        }

        public int SMaterialId { get; set; }
        public string SMaterial { get; set; }

        public virtual List<Product> Products { get; set; }
    }
}
