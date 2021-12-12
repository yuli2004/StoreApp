using StoreApp.Models;
using StoreApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Forms;

namespace StoreApp.ViewModels
{
    public static class ERROR_MESSAGES
    {
        public const string REQUIRED_FIELD = "זהו שדה חובה";
        public const string SHORT_PASS = "סיסמה חייבת להכיל לפחות 6 תווים";
        public const string BAD_EMAIL = "מייל לא תקין";
    }
    class RegisterBuyerViewModel : BaseViewModel
    {
        #region username
        private bool showUsernameError;

        public bool ShowUsernameError
        {
            get => showUsernameError;
            set
            {
                showUsernameError = value;
                OnPropertyChanged("ShowUsernameError");
            }
        }

        private string username;

        public string Username
        {
            get => username;
            set
            {
                username = value;
                ValidateUsername();
                OnPropertyChanged("Username");
            }
        }

        private string usernameError;

        public string UsernameError
        {
            get => usernameError;
            set
            {
                usernameError = value;
                OnPropertyChanged("UsernameError");
            }
        }

        private void ValidateUsername()
        {
            this.ShowUsernameError = string.IsNullOrEmpty(username);
        }
        #endregion
        #region email
        private bool showEmailError;

        public bool ShowEmailError
        {
            get => showEmailError;
            set
            {
                showEmailError = value;
                OnPropertyChanged("ShowEmailError");
            }
        }

        private string email;

        public string Email
        {
            get => email;
            set
            {
                email = value;
                ValidateEmail();
                OnPropertyChanged("Email");
            }
        }

        private string emailError;

        public string EmailError
        {
            get => emailError;
            set
            {
                emailError = value;
                OnPropertyChanged("EmailError");
            }
        }

        private void ValidateEmail()
        {
            this.ShowEmailError = string.IsNullOrEmpty(Email);
            if (!this.ShowEmailError)
            {
                if (!Regex.IsMatch(this.Email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
                {
                    this.ShowEmailError = true;
                    this.EmailError = ERROR_MESSAGES.BAD_EMAIL;
                }
            }
        }
        #endregion
        #region password
        private bool showPasswordError;

        public bool ShowPasswordError
        {
            get => showPasswordError;
            set
            {
                showPasswordError = value;
                OnPropertyChanged("ShowPasswordError");
            }
        }

        private string password;

        public string Password
        {
            get => password;
            set
            {
                password = value;
                ValidatePassword();
                OnPropertyChanged("Password");
            }
        }

        private string passwordError;

        public string PasswordError
        {
            get => passwordError;
            set
            {
                passwordError = value;
                OnPropertyChanged("PasswordError");
            }
        }

        private void ValidatePassword()
        {
            this.ShowPasswordError = string.IsNullOrEmpty(Password);
            if (!this.ShowPasswordError)
            {
                if (this.Password.Length < 6)
                {
                    this.ShowPasswordError = true;
                    this.PasswordError = ERROR_MESSAGES.SHORT_PASS;
                }
            }
        }
        #endregion
        #region isAdmin
        private bool isAdmin;
        public bool IsAdmin
        {
            get => isAdmin;
            set
            {
                isAdmin = value;

                OnPropertyChanged("IsAdmin");
            }
        }
        #endregion
        #region isBuyer
        private bool isBuyer;
        public bool IsBuyer
        {
            get => isBuyer;
            set
            {
                isBuyer = value;

                OnPropertyChanged("IsBuyer");
            }
        }
        #endregion
        #region isSeller
        private bool isSeller;
        public bool IsSeller
        {
            get => isSeller;
            set
            {
                isSeller = value;

                OnPropertyChanged("IsSeller");
            }
        }
        #endregion

        User u;
        Buyer b;

        #region constructor
        public RegisterBuyerViewModel()
        {
            App theApp = (App)App.Current;
            u = new User()
            {
                Username = "",
                Email = "",
                Password = "",
                IsAdmin = false,
                IsBuyer = true,
                IsSeller = false,
            };
            b = new Buyer()
            { UsernameNavigation = u };

            this.UsernameError = ERROR_MESSAGES.REQUIRED_FIELD;
            this.PasswordError = ERROR_MESSAGES.SHORT_PASS;
            this.EmailError = ERROR_MESSAGES.BAD_EMAIL;

            this.ShowUsernameError = false;
            this.ShowEmailError = false;
            this.ShowPasswordError = false;

            this.SaveDataCommand = new Command(() => SaveData());

        }
        #endregion

        #region ValidateForm
        //This function validate the entire form upon submit!
        private bool ValidateForm()
        {
            //Validate all fields first
            ValidateUsername();
            ValidateEmail();
            ValidatePassword();

            //check if any validation failed
            if (ShowUsernameError || ShowEmailError || ShowPasswordError)
                return false;
            return true;
        }
        #endregion

        #region ServerStatus
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
        #endregion

        #region SaveData

        //The command for saving the contact
        public ICommand SaveDataCommand { protected set; get; }
        private async void SaveData()
        {
            if (ValidateForm())
            {
                this.b.Username = this.Username;

                this.u.Username = this.Username;
                this.u.Email = this.Email;
                this.u.Password = this.Password;

                ServerStatus = "מתחבר לשרת...";
                await App.Current.MainPage.Navigation.PushModalAsync(new Views.ServerStatusPage(this));
                StoreAPIProxy proxy = StoreAPIProxy.CreateProxy();


                //the email and username should be unique
                bool isEmailExists = await proxy.UserExistsByEmailAsync(this.u.Email);
                bool isUsernameExists = await proxy.UserExistsByUsernameAsync(this.u.Username);
                if (isUsernameExists)
                {
                    await App.Current.MainPage.DisplayAlert("שגיאה", "שם המשתמש תפוס - נסה שם אחר", "בסדר");
                    await App.Current.MainPage.Navigation.PopModalAsync();
                }
                if (isEmailExists)
                {
                    await App.Current.MainPage.DisplayAlert("שגיאה", "האימייל כבר נמצא בשימוש", "בסדר");
                    await App.Current.MainPage.Navigation.PopModalAsync();
                }
                if (!isUsernameExists && !isEmailExists)
                {
                    Buyer currentB = null;
                    Buyer newB = await proxy.RegisterBuyerAsync(this.b);
                    currentB = newB;

                    if (currentB == null)
                    {
                        await App.Current.MainPage.DisplayAlert("שגיאה", "שמירת המשתמש נכשלה", "בסדר");
                        await App.Current.MainPage.Navigation.PopModalAsync();
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("הצלחה", "שמירת המשתמש הצליחה", "בסדר");
                        await App.Current.MainPage.Navigation.PopModalAsync();
                    }
                }

            }
        }
        #endregion
    }
}


