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
using System.Threading.Tasks;

namespace StoreApp.ViewModels
{
   public class ProductViewModel:BaseViewModel
    {
        public Models.Product P { get; set; }

        private bool isOnSale;
        public bool IsOnSale
        {
            get => isOnSale;
            set
            {
                if(value != isOnSale)
                {
                    isOnSale = value;
                    UpdateProduct();
                    OnPropertyChanged("IsOnSale");
                }
            }
        }

        private async Task UpdateProduct()
        {
            P.IsActive = IsOnSale;
            StoreAPIProxy proxy = StoreAPIProxy.CreateProxy();
           await proxy.UpdateProduct(P);
        }

        public bool IsOwner
        {
            get
            {
                if (currentApp.CurrentUser != null) 
                    return P.Seller.Username == currentApp.CurrentUser.Username;

                return false;
            }
            
        }

        public ProductViewModel()
        {
            AddToCart = new Command(AddProduct);

        }
       
        public ICommand AddToCart { get; protected set; }

        public async void AddProduct()
        {
            
            ProductInOrder product = new ProductInOrder() { Product = P };
            if (currentApp.cart.Count > 0)
                product.Order = currentApp.cart[0].Order;
            
                currentApp.cart.Add(product);
            await this.currentApp.MainPage.DisplayAlert("הוספה לסל הקניות", $"{P.ProductName} נוסף לעגלה","אישור");
            await this.currentApp.MainPage.Navigation.PopAsync();


        }

   }
}
