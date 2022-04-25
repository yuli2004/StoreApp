using StoreApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StoreApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SellerProfile : ContentPage
    {
        public SellerProfile()
        {
            InitializeComponent();
            
        }
        public SellerProfile(SellerProfileViewModel vm)
        {
            BindingContext = vm;
            InitializeComponent();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext != null)
            {
                ((SellerProfileViewModel)BindingContext).IsSeller = (((App)Application.Current).CurrentUser.Seller == );
            }

        }
    }
}