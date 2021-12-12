using System;
using System.Collections.Generic;
using System.Text;

namespace StoreApp.Models
{
    public partial class Material
    {
        public Material()
        {
            Products = new List<Product>();
        }

        public int MaterialId { get; set; }
        public string Material1 { get; set; }

        public virtual List<Product> Products { get; set; }
    }
}
