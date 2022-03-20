using StoreApp.Models;
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
            BuyAllCommand = new Command(BuyCart);
        }

        private void BuyCart()
        {
            Order o = new Order();
            foreach (ProductInOrder p in Order)
            {
                p.Product.IsActive = false;  
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

        public ICommand BuyAllCommand { get; protected set; }
    }
}
