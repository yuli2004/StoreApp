using StoreApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StoreApp.ViewModels
{
   public class ProductViewModel:BaseViewModel
    {
        public Product p { get; set; } 
        public ProductViewModel(Product current)
        {
            p = current;
        }

        public ProductViewModel() { }

    }
}
