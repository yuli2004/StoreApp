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
                ((HomeViewModel)BindingContext).IsVisible = (((App)Application.Current).CurrentUser == null);
                ((HomeViewModel)BindingContext).InitProducts();
            }
                
        }

    }
}