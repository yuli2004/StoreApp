using StoreApp.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using StoreApp.Services;

namespace StoreApp.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        protected StoreAPIProxy proxy;
        public  App currentApp { get => ((App)Application.Current); }
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public BaseViewModel()
        {
            NavigateToPageCommand = new Command<string>(NavigateToPage);
            proxy = StoreAPIProxy.CreateProxy();
        }

        #region Navigate to page command
        public ICommand NavigateToPageCommand { protected set; get; }
        
        public void NavigateToPage(string obj)
        {
            Page p = null;
            switch(obj)
            {
                case "LogIn":
                    {
                        p = new LogIn();
                        p.BindingContext = new LogInViewModel();
                    }
                    break;
                case "RegisterBuyer":
                    {
                        p = new RegisterBuyer();
                        p.BindingContext = new RegisterBuyerViewModel();
                    }
                    break;
                case "RegisterSeller":
                    {
                        RegisterSellerViewModel context = new RegisterSellerViewModel();
                        p = new RegisterSeller(context);
                        p.BindingContext = context;
                    }
                    break;
                case "Home":
                    {
                        p = new Home();
                        p.BindingContext = new HomeViewModel();
                    }
                    break;
                case "ShoppingCart":
                    { 
                        p = new ShoppingCartPage();
                        p.BindingContext = new ShoppingCartPageViewModel();
                    }
                    break;
               
                default: break;
            }
            currentApp.MainPage.Navigation.PushAsync(p);
        }
        #endregion
    
    }
}
