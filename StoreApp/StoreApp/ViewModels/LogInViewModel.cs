using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using StoreApp.Services;
using StoreApp.Models;
using Xamarin.Essentials;
using System.Linq;
using Xamarin.Forms.Xaml;
using StoreApp.Views;


namespace StoreApp.ViewModels
{
    class LogInViewModel: BaseViewModel
    {
        #region username
        private string username;
        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                OnPropertyChanged("Username");
            }
        }
        #endregion

        #region password
        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }
        #endregion

        public ICommand LogInCommand { protected set; get; }

        public LogInViewModel()
        {
            LogInCommand = new Command(LogInSubmit);
            
        }

        private string serverStatus;
        public string ServerStatus
        {
            get { return serverStatus; }
            set
            {
                serverStatus = value;
                OnPropertyChanged("ServerStatus");
            }
        }

        public async void LogInSubmit()
        {
            ServerStatus = "מתחבר לשרת...";
            //await App.Current.MainPage.Navigation.PushModalAsync(new Views.ServerStatusPage(this));
            StoreAPIProxy proxy = StoreAPIProxy.CreateProxy();
            User user = await proxy.LogInAsync(username, Password);
            if (user == null)
            {
                await App.Current.MainPage.Navigation.PopModalAsync();
                await App.Current.MainPage.DisplayAlert("שגיאה", "התחברות נכשלה, בדוק שם משתמש וסיסמה ונסה שוב", "בסדר");
            }
            else
            {
                ServerStatus = "קורא נתונים...";
                App theApp = (App)App.Current;
                theApp.CurrentUser = user;
                await App.Current.MainPage.DisplayAlert("הצלחה", "התחברות הצליחה", "בסדר");
                await theApp.MainPage.Navigation.PopToRootAsync();
            }
        }
    }
}
