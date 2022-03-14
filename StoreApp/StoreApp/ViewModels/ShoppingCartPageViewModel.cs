using StoreApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

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
    }
}
