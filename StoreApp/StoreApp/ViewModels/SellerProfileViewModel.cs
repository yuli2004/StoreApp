using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace StoreApp.ViewModels
{
    public class SellerProfileViewModel : BaseViewModel
    {
        public Models.Seller Sl { get; set; }

        #region constructor
        public SellerProfileViewModel()
        {
            SellerProducts = new ObservableCollection<Models.Product>();
            //SellerProducts = new ObservableCollection<Models.Product>(((App)App.Current).Tables.AllProducts.Where(p => p.SellerId == S.SellerId && p.IsActive == true));
            
            OnSelectedProduct = new Command<Models.Product>(MoveToProductPage);

        }
        #endregion

        #region products list

        private ObservableCollection<Models.Product> sellerProducts;
        public ObservableCollection<Models.Product> SellerProducts
        {
            get 
            {                
                return new ObservableCollection<Models.Product>(Sl.Products.Where(p => p.IsActive == true));
            }
            set
            {
                if (this.sellerProducts != value)
                {
                    this.sellerProducts = value;
                    OnPropertyChanged("SellerProducts");
                }
            }
        }

        #endregion

        #region Selected Product
        private Models.Product selectedProduct;
        public Models.Product SelectedProduct
        {
            get => selectedProduct;
            set
            {
                if (value != selectedProduct)
                {
                    selectedProduct = value;
                    OnPropertyChanged("SelectedProduct");
                }
            }
        }
        #endregion

        #region open product page
        public ICommand OnSelectedProduct { get; protected set; }
        private async void MoveToProductPage(Models.Product obj)
        {
            if (SelectedProduct != null)
            {
                var page = new Views.Product() { Title = "פרטי מוצר" };
                var binding = new ProductViewModel() { P = obj };
                page.BindingContext = binding;
                await this.currentApp.MainPage.Navigation.PushAsync(page);
                SelectedProduct = null;
            }
        }

        #endregion

        #region IsSeller
        private bool isSeller;
        public bool IsSeller
        {
            get
            {
                if (currentApp.CurrentUser == null)
                    return false;
                else if (currentApp.CurrentUser.Seller == Sl)
                {
                    return true;
                }
                return false;
            }
            set
            {
                if (isSeller != value)
                {
                    isSeller = value;
                    OnPropertyChanged("IsSeller");
                }
            }
        }
        #endregion

    }
}
