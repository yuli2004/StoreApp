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
        public ShoppingCartPageViewModel()
        {
            Price = 0;
            Order = new ObservableCollection<ProductInOrder>(currentApp.cart);
            UpdatePrice();
            DeleteCommand = new Command<ProductInOrder>(DeleteFromCart);
            OrderCommand = new Command(BuyCart);
        }

        private async void BuyCart()
        {
            if (currentApp.CurrentUser == null)
            {
                await currentApp.MainPage.DisplayAlert("משתמש לא מזוהה", "על מנת לבצע הזמנה, יש להתחבר למערכת", "אישור");
                await currentApp.MainPage.Navigation.PushAsync(new LogIn());
            }
            else
            {
                Order o = new Order() { Buyer = currentApp.CurrentUser.Buyer, Date=DateTime.Now, ProductInOrders= currentApp.cart, TotalPrice=Price };
                bool success = await proxy.CreateOrder(o);
                if(success)
                {
                    currentApp.cart.Clear();
                    order.Clear();
                    currentApp.Tables= await proxy.CreateLookUpTables();
                    await currentApp.MainPage.DisplayAlert("ההזמנה בוצעה", "תודה על ההזמנה שלך!", "אישור");

                    await currentApp.MainPage.Navigation.PopAsync();
                }
                else
                    await currentApp.MainPage.DisplayAlert("הזמנה נכשלה", "ביצוע הזמנה נכשל", "אישור");
            }
        }

        private void DeleteFromCart(ProductInOrder obj)
        {
            currentApp.cart.Remove(obj);
            Order.Remove(obj);
            Price -= obj.Product.Price;
        }

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

      public  ICommand DeleteCommand { get; protected set; }

        public ICommand OrderCommand { get; protected set; }
    }
}
