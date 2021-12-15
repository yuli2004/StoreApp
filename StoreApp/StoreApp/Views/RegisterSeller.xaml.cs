using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using StoreApp.Models;
using StoreApp.ViewModels;

namespace StoreApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterSeller : ContentPage
    {
        public RegisterSeller(RegisterSellerViewModel context)
        {
            InitializeComponent();
        }
        
    }
}