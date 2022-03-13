using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using StoreApp.Models;
using StoreApp.Services;
using StoreApp.Views;

namespace StoreApp.ViewModels
{
   public class ProductViewModel:BaseViewModel
    {
        public Models.Product P { get; set; }
        

        public ProductViewModel()
        {
            
                }
       
        public ICommand AddToCart => new Command(AddProduct);

        public async void AddProduct()
        {
            ProductInOrder 
        }
   }
}
