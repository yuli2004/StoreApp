using StoreApp.Models;
using StoreApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace StoreApp.ViewModels
{
    class ShoppingCartPageViewModel:BaseViewModel
    {
        #region price
        private double price;
        public double Price
        {
            get => price;
            set
            {
                price = value;
                OnPropertyChanged("Price");
            }
        }
        #endregion

        #region isCart
        private bool isCart;
        public bool IsCart
        {
            get
            {
                return Price != 0;
            }
            set
            {
                if (isCart != value)
                {
                    isCart = value;
                    OnPropertyChanged("IsCart");
                }
            }

        }
        #endregion

        #region order
        private ObservableCollection<ProductInOrder> order;
        public ObservableCollection<ProductInOrder> Order
        {
            get => order;

            set
            {
                if (value != null)
                {
                    order = value;
                    OnPropertyChanged("Order");
                }
            }
        }
        #endregion

        #region constructor
        public ShoppingCartPageViewModel()
        {
            Price = 0;
            Order = new ObservableCollection<ProductInOrder>(currentApp.cart);
            UpdatePrice();
            DeleteCommand = new Command<ProductInOrder>(DeleteFromCart);
            OrderCommand = new Command(BuyCart);
        }
        #endregion

        #region buy cart
        public ICommand OrderCommand { get; protected set; }
        private async void BuyCart()
        {
            if (currentApp.CurrentUser == null)
            {
                await currentApp.MainPage.DisplayAlert("משתמש לא מזוהה", "על מנת לבצע הזמנה, יש להתחבר למערכת", "אישור");
                await currentApp.MainPage.Navigation.PushAsync(new LogIn());
            }
            else
            {
                List<ProductInOrder> a = new List<ProductInOrder>(currentApp.cart);
                Order o = new Order() { Buyer = currentApp.CurrentUser.Buyer, Date=DateTime.Now, ProductInOrders= a, TotalPrice=Price };
                bool success = await proxy.CreateOrder(o);
                if(success)
                {
                    
                    foreach (ProductInOrder p in o.ProductInOrders)
                    {
                        ((App)App.Current).Tables.SoldProducts.Add(p);
                        p.Product.IsActive = false;
                    }
                    currentApp.CurrentUser.Buyer.Orders.Add(o);
                    currentApp.cart.Clear();
                    order.Clear();
                    currentApp.Tables.AllProducts = await proxy.GetSearchResults(string.Empty); 
                    await currentApp.MainPage.DisplayAlert("ההזמנה בוצעה", "תודה על ההזמנה שלך!", "אישור");

                    await currentApp.MainPage.Navigation.PopAsync();
                }
                else
                    await currentApp.MainPage.DisplayAlert("הזמנה נכשלה", "ביצוע הזמנה נכשל", "אישור");
            }
        }
        #endregion

        #region delete from cart
        public ICommand DeleteCommand { get; protected set; }
        private void DeleteFromCart(ProductInOrder obj)
        {
            currentApp.cart.Remove(obj);
            Order.Remove(obj);
            Price -= obj.Product.Price;
            if (Price == 0)
                IsCart = false;
        }
        #endregion

        #region update price
        private void UpdatePrice()
        {
           if(Order.Count>0)
           {
                foreach(ProductInOrder p in Order)
                {
                    Price += p.Product.Price;
                }
           }
        }
        #endregion
        
    }
}
