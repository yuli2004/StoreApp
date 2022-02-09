using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using StoreApp.Models;
using StoreApp.Views;
using System.Collections.Generic;
using StoreApp.Services;

namespace StoreApp
{
    public partial class App : Application
    {
        public static bool IsDevEnv
        {
            get
            {
                return true; //change this before release!
            }
        }

        //The current logged in user
        public User CurrentUser { get; set; }
        public LookupTables Tables { get; set; }


        //The list of phone types
        //public List<PhoneType> PhoneTypes { get; set; }
        public App()
        {
            InitializeComponent();
            CurrentUser = null;
            //PhoneTypes = new List<PhoneType>();
            MainPage = new  Loading();
        }

        protected async override void OnStart()
        {
            StoreAPIProxy proxy = StoreAPIProxy.CreateProxy();
           
            Tables = await proxy.CreateLookUpTables();
            
            MainPage = new NavigationPage(new Home());
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
