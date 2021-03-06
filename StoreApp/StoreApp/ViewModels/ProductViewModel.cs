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
        public string Img
        {
            get
            {
                if (P != null) return P.GetPicture();
                else return null;
            }
        }

        #region is on sale
        private bool isOnSale;
        public bool IsOnSale
        {
            get{ return P.IsActive; }
            set
            {
                if (value != isOnSale)
                {
                    isOnSale = value;
                    UpdateProduct();
                    OnPropertyChanged("IsOnSale");
                }
            }
        }
        #endregion

        #region is for sale
        private bool isForSale;
        public bool IsForSale
        {
            get 
            {
                if (currentApp.CurrentUser != null)
                {
                    if (currentApp.CurrentUser.IsSeller)
                        return false;
                }
                return P.IsActive; 
            }
            set
            {
                if (value != isForSale)
                {
                    isForSale = value;
                    P.IsActive = value;
                    OnPropertyChanged("IsForSale");
                }
            }
        }
        #endregion

        #region update product
        private async Task UpdateProduct()
        {
            P.IsActive = IsOnSale;
            StoreAPIProxy proxy = StoreAPIProxy.CreateProxy();
            await proxy.UpdateProduct(P);
        }
        #endregion

        #region is owner
        public bool IsOwner
        {
            get
            {
                if (currentApp.CurrentUser != null)
                {
                    if (P.IsActive == false)
                        return false;
                    return P.Seller.Username == currentApp.CurrentUser.Username;
                }

                return false;
            }
            
        }
        #endregion

        #region constructor
        public ProductViewModel()
        {
            AddToCart = new Command(AddProduct);
            OnSelectedSeller = new Command(MoveToSellerPage);
            DeleteProduct = new Command(RemoveProduct);
        }
        #endregion

        #region add to cart
        public ICommand AddToCart { get; protected set; }
        public async void AddProduct()
        {           
            ProductInOrder product = new ProductInOrder() { Product = P };

            foreach(ProductInOrder pr in currentApp.cart)
            {
                if(P.ProductId == pr.Product.ProductId)
                {
                    await this.currentApp.MainPage.DisplayAlert("שגיאה", $"המוצר {P.ProductName} כבר נמצא בעגלת הקניות", "אישור");
                    return;
                }
            }

            if (currentApp.cart.Count > 0)
                product.Order = currentApp.cart[0].Order;
            
            currentApp.cart.Add(product);
            await this.currentApp.MainPage.DisplayAlert("הוספה לסל הקניות", $"{P.ProductName} נוסף לעגלה","אישור");
            await this.currentApp.MainPage.Navigation.PopAsync();
        }
        #endregion

        #region delete product
        public ICommand DeleteProduct { get; protected set; }
        public async void RemoveProduct()
        {
            bool success = await proxy.RemoveProduct(P);
            P.IsActive = false;         
            IsOnSale = false;
            IsForSale = false;
            await currentApp.MainPage.Navigation.PopAsync();
        }
        #endregion

        #region move to Seller profile

        public ICommand OnSelectedSeller { get; protected set; }

        private async void MoveToSellerPage()
        {
            Seller s = P.Seller;
            SellerProfileViewModel sVM = new SellerProfileViewModel() { Sl = s };
            var page = new Views.SellerProfile(sVM) {  Title = "פרופיל מוכר" };
          
            await this.currentApp.MainPage.Navigation.PushAsync(page);
        }
        #endregion

    }
}
