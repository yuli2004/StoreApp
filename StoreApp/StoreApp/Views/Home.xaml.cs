using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using StoreApp.Models;

namespace StoreApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home : ContentPage
    {
        public Home()
        {
            HomeViewModel context = new HomeViewModel();

            this.BindingContext = context;
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext != null)
            {
                ((HomeViewModel)BindingContext).IsNoUser = (((App)Application.Current).CurrentUser == null);
                ((HomeViewModel)BindingContext).IsLoggedUser = (((App)Application.Current).CurrentUser != null);
                ((HomeViewModel)BindingContext).IsBuyer = ((HomeViewModel)BindingContext).IsLoggedUser&&(((App)Application.Current).CurrentUser.IsBuyer);
                ((HomeViewModel)BindingContext).IsSeller = ((HomeViewModel)BindingContext).IsLoggedUser&&(((App)Application.Current).CurrentUser.IsSeller);
                ((HomeViewModel)BindingContext).IsNoSeller = ((HomeViewModel)BindingContext).IsNoUser || ((HomeViewModel)BindingContext).IsBuyer;
                ((HomeViewModel)BindingContext).InitProducts();
            }
                
        }

    }
}