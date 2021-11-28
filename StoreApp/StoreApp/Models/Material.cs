using System;
using System.Collections.Generic;
using System.Text;

namespace StoreApp.Models
{
    public partial class Material
    {
        public Material()
        {
            Products = new HashSet<Product>();
        }

        public int MaterialId { get; set; }
        public string Material1 { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
