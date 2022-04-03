using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace StoreApp.ViewModels
{
    class BuyerHistoryViewModel: BaseViewModel
    {
        #region buyer orders list
        private ObservableCollection<Models.Order> buyerOrders;
        public ObservableCollection<Models.Order> BuyerOrders
        {
            get
            {
                return this.buyerOrders;
            }
            set
            {
                if (this.buyerOrders != value)
                {

                    this.buyerOrders = value;
                    OnPropertyChanged("BuyerOrders");
                }
            }
        }
        #endregion

        #region Selected Order
        private Models.Order selectedOrder;
        public Models.Order SelectedOrder
        {
            get => selectedOrder;
            set
            {
                if (value != selectedOrder)
                {
                    selectedOrder = value;
                    OnPropertyChanged("SelectedOrder");
                }
            }
        }
        #endregion

        #region open Order page

        public ICommand OnSelectedOrder { get; protected set; }

        #endregion

        #region constructor
        public BuyerHistoryViewModel()
        {
            
            BuyerOrders = new ObservableCollection<Models.Order>(((App)App.Current).CurrentUser.Buyer.Orders);
            OnSelectedOrder = new Command<Models.Order>(MoveToOrderPage);
        }

        private async void MoveToOrderPage(Models.Order obj)
        {
            if (SelectedOrder != null)
            {
                var page = new Views.ViewOrder() { Title = "View Order Page" };
                var binding = new ViewOrderViewModel() { O = obj };
                page.BindingContext = binding;
                await this.currentApp.MainPage.Navigation.PushAsync(page);
                SelectedOrder = null;
            }
        }
        #endregion
    }
}
