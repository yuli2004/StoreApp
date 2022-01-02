using StoreApp.Models;
using StoreApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace StoreApp.ViewModels
{
    public class RegisterSellerViewModel: BaseViewModel
    {
        #region userName/storeName
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
        #region name
        private bool showNameError;

        public bool ShowNameError
        {
            get => showNameError;
            set
            {
                showNameError = value;
                OnPropertyChanged("ShowNameError");
            }
        }

        private string name;

        public string Name
        {
            get => name;
            set
            {
                name = value;
                ValidateName();
                OnPropertyChanged("Name");
            }
        }

        private string nameError;

        public string NameError
        {
            get => nameError;
            set
            {
                nameError = value;
                OnPropertyChanged("NameError");
            }
        }

        private void ValidateName()
        {
            this.ShowNameError = string.IsNullOrEmpty(name);
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
        #region info
        private bool showInfoError;

        public bool ShowInfoError
        {
            get => showInfoError;
            set
            {
                showInfoError = value;
                OnPropertyChanged("ShowInfoError");
            }
        }

        private string info;

        public string Info
        {
            get => info;
            set
            {
                info = value;
                ValidateInfo();
                OnPropertyChanged("Info");
            }
        }

        private string infoError;

        public string InfoError
        {
            get => infoError;
            set
            {
                infoError = value;
                OnPropertyChanged("InfoError");
            }
        }

        private void ValidateInfo()
        {
            this.ShowInfoError = string.IsNullOrEmpty(info);
        }
        #endregion
        #region picture
        private string sellerImgSrc;

        public string SellerImgSrc
        {
            get => sellerImgSrc;
            set
            {
                sellerImgSrc = value;
                OnPropertyChanged("SellerImgSrc");
            }
        }
        private const string DEFAULT_PHOTO_SRC = "defaultphoto.png";
        #endregion

        User u;
        Seller s;

        #region constructor
        public RegisterSellerViewModel()
        {
            App theApp = (App)App.Current;
            u = new User()
            {
                Username = "",                
                Email = "",
                Password = "",
                IsAdmin = false,
                IsBuyer = false,
                IsSeller = true,
            };
            s = new Seller()
            { UsernameNavigation = u };

            //Setup default image photo
            this.SellerImgSrc = StoreAPIProxy.GetImageURL() + DEFAULT_PHOTO_SRC;
            this.imageFileResult = null; //mark that no picture was chosen

            this.UsernameError = ERROR_MESSAGES.REQUIRED_FIELD;
            this.PasswordError = ERROR_MESSAGES.SHORT_PASS;
            this.EmailError = ERROR_MESSAGES.BAD_EMAIL;
            this.InfoError = ERROR_MESSAGES.REQUIRED_FIELD;
            this.nameError = ERROR_MESSAGES.REQUIRED_FIELD;

            this.ShowUsernameError = false;
            this.ShowEmailError = false;
            this.ShowPasswordError = false;
            this.showInfoError = false;
            this.showNameError = false;            

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
            ValidateInfo();
            ValidateName();

            //check if any validation failed
            if (ShowUsernameError || ShowEmailError || ShowPasswordError || ShowInfoError || ShowNameError)
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

                this.s.Username = this.Username;
                this.s.Name = this.Name;
                this.s.Info = this.Info;
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
                    Seller currentS = null;
                    Seller newS = await proxy.RegisterSellerAsync(this.s);
                    currentS = newS;

                    if (currentS == null)
                    {
                        await App.Current.MainPage.DisplayAlert("שגיאה", "שמירת המשתמש נכשלה", "בסדר");
                        await App.Current.MainPage.Navigation.PopModalAsync();
                    }
                    else
                    {
                        if (this.imageFileResult != null)
                        {
                            ServerStatus = "מעלה תמונה...";

                            bool success = await proxy.UploadImage(new FileInfo()
                            {
                                Name = this.imageFileResult.FullPath
                            }, $"{currentS.Picture}.jpg");
                        }
                        ServerStatus = "שומר נתונים..."; 

                        await App.Current.MainPage.DisplayAlert("הצלחה", "שמירת המשתמש הצליחה", "בסדר");
                        await App.Current.MainPage.Navigation.PopModalAsync();
                    }
                }

            }
        }
        #endregion

        #region pick image command

        FileResult imageFileResult;
        public event Action<ImageSource> SetImageSourceEvent;
        public ICommand PickImageCommand => new Command(OnPickImage);
        public async void OnPickImage()
        {
            FileResult result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions()
            {
                Title = "בחר תמונה"
            });

            if (result != null)
            {
                this.imageFileResult = result;

                var stream = await result.OpenReadAsync();
                ImageSource imgSource = ImageSource.FromStream(() => stream);
                this.SellerImgSrc = result.FullPath;
                if (SetImageSourceEvent != null)
                    SetImageSourceEvent(imgSource);
            }
        }
        #endregion

        #region take photo command

        //The following command handle the take photo button
        public ICommand CameraImageCommand => new Command(OnCameraImage);
        public async void OnCameraImage()
        {
            var result = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions()
            {
                Title = "צלם תמונה"
            });

            if (result != null)
            {
                this.imageFileResult = result;
                var stream = await result.OpenReadAsync();
                ImageSource imgSource = ImageSource.FromStream(() => stream);
                this.SellerImgSrc = result.FullPath;
                if (SetImageSourceEvent != null)
                    SetImageSourceEvent(imgSource);
            }
        }
        #endregion
    }
}
