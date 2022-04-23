using StoreApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace StoreApp.ViewModels
{
    class SellerHistoryViewModel: BaseViewModel
    {
        #region Seller sold products list
        private ObservableCollection<Models.ProductInOrder> sellerSoldProducts;
        public ObservableCollection<Models.ProductInOrder> SellerSoldProducts
        {
            get
            {
                return this.sellerSoldProducts;
            }
            set
            {
                if (this.sellerSoldProducts != value)
                {

                    this.sellerSoldProducts = value;
                    OnPropertyChanged("SellerSoldProducts");
                }
            }
        }
        #endregion

        #region constructor
        public SellerHistoryViewModel()
        {
            sellerSoldProducts = new ObservableCollection<Models.ProductInOrder>();
            foreach (Product pr in currentApp.CurrentUser.Seller.Products)
            {
                foreach(ProductInOrder p in ((App)App.Current).Tables.SoldProducts)
                {
                    if (p.ProductId == pr.ProductId)
                        sellerSoldProducts.Add(p);
                }
            }
            
            OnSelectedProduct = new Command<Models.ProductInOrder>(MoveToProductPage);
        }
        #endregion

        #region Selected Product
        private Models.ProductInOrder selectedProduct;
        public Models.ProductInOrder SelectedProduct
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

        #region open Product page

        public ICommand OnSelectedProduct { get; protected set; }

        private async void MoveToProductPage(Models.ProductInOrder obj)
        {
            if (SelectedProduct != null)
            {
                var page = new Views.Product() { Title = "פרטי מוצר" };
                var binding = new ProductViewModel() { P = obj.Product };
                page.BindingContext = binding;
                await this.currentApp.MainPage.Navigation.PushAsync(page);
                SelectedProduct = null;
            }
        }
        #endregion
    }
}
