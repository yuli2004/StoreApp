using StoreApp.Models;
using System;
using System.Collections.Generic;
using StoreApp.Models;
using System.Text;

namespace StoreApp.ViewModels
{
   public class ProductViewModel:BaseViewModel
    {
        public Product P { get; set; }

        //private double price;
        //public double Price { get => P.Price; }

      

        public ProductViewModel() { }

    }
}
