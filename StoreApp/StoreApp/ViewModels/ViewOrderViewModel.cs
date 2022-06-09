using StoreApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace StoreApp.ViewModels
{
    class ViewOrderViewModel:BaseViewModel
    {
        private Models.Order o;
        public Models.Order O { get { return o; } set { o = value; InitProducts(); }
            }

        private void InitProducts()
        {
            if(o!=null)
            {
                ProductsInOrderList = O.ProductInOrders;
                ProductsList = new ObservableCollection<Models.Product>();
                foreach (Models.ProductInOrder p in ProductsInOrderList)
                {
                    int id = p.ProductId;
                    ProductsList.Add((((App)App.Current).Tables.SoldProducts).FirstOrDefault(pr => pr.ProductId == id).Product);
                }
            }
        }
    
       
        #region order productsInOrder list 
        private List<Models.ProductInOrder> productsInOrderList;
        public List<Models.ProductInOrder> ProductsInOrderList
        {
            get
            {
                if (O != null)
                    return O.ProductInOrders;
                else return null;
            }
            set
            {
                if (this.productsInOrderList != value)
                {

                    this.productsInOrderList = value;
                    OnPropertyChanged("ProductsInOrderList");
                }
            }
        }
        #endregion

        #region order products list
        private ObservableCollection<Models.Product> productsList;
        public ObservableCollection<Models.Product> ProductsList
        {
            get
            {
               
                return this.productsList;
            }
            set
            {
                if (this.productsList != value)
                {
                    this.productsList = value;
                    OnPropertyChanged("ProductsList");
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

        #region constructor
        public ViewOrderViewModel()
        {
            ProductsInOrderList = new List<Models.ProductInOrder>();
            OnSelectedProduct = new Command<Models.Product>(MoveToProductPage);
            ProductsList = new ObservableCollection<Models.Product>();

        }
        #endregion

        #region update order
        private async Task UpdateOrder()
        {
            StoreAPIProxy proxy = StoreAPIProxy.CreateProxy();
            await proxy.UpdateOrder(O);
            productsInOrderList = new List<Models.ProductInOrder>(O.ProductInOrders);
        }
        #endregion

        #region open Product page

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
    }
}
