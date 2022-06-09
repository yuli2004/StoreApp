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
    class EditProfileViewModel : BaseViewModel
    {
        #region name       
        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
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

                    if (this.Email!=null&&!Regex.IsMatch(this.Email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
                    {
                        this.ShowEmailError = true;
                        this.EmailError = ERROR_MESSAGES.BAD_EMAIL;
                    }
            else
            {
                this.ShowEmailError = false;
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
            
                if (this.Password!=null && this.Password.Length < 6)
                {
                    this.ShowPasswordError = true;
                    this.PasswordError = ERROR_MESSAGES.SHORT_PASS;
                }
                else
                {
                    this.ShowPasswordError = false;
                }
            
        }
        #endregion
        #region info      
        private string info;
        public string Info
        {
            get => info;
            set
            {
                info = value;
                OnPropertyChanged("Info");
            }
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

        #region constructor
        public EditProfileViewModel()
        {
            App theApp = (App)App.Current;
            this.SellerImgSrc = StoreAPIProxy.GetImageURL() + DEFAULT_PHOTO_SRC;
            this.imageFileResult = null; //mark that no picture was chosen

            this.PasswordError = ERROR_MESSAGES.SHORT_PASS;
            this.EmailError = ERROR_MESSAGES.BAD_EMAIL;

            this.ShowEmailError = false;
            this.ShowPasswordError = false;

            this.SaveDataCommand = new Command(() => SaveData());
        }
        #endregion

        #region SaveData

        //The command for saving the contact
        public ICommand SaveDataCommand { protected set; get; }
        private async void SaveData()
        {
            if (ValidateForm())
            {
                User u = currentApp.CurrentUser;
                Seller s = currentApp.CurrentUser.Seller;
                StoreAPIProxy proxy = StoreAPIProxy.CreateProxy();

                if (Name != null)
                    s.Name = Name;
                if (Email != null)
                    u.Email = Email;
                if (Password != null)
                    u.Password = Password;
                if (Info != null)
                    s.Info = info;
                if(!SellerImgSrc.Contains(DEFAULT_PHOTO_SRC))
                {
                    s.Picture = SellerImgSrc;
                    ServerStatus = "מעלה תמונה...";

                    bool success = await proxy.UploadImage(new FileInfo()
                    {
                        Name = this.imageFileResult.FullPath
                    }, $"{s.SellerId}.jpg");
                }
                    

                ServerStatus = "מתחבר לשרת...";
                await App.Current.MainPage.Navigation.PushModalAsync(new Views.ServerStatusPage(this));
                

                //the email should be unique
                bool isEmailExists = await proxy.UserExistsByEmailAsync(u.Email);
                if (Email != null && isEmailExists)
                {
                    await App.Current.MainPage.DisplayAlert("שגיאה", "האימייל כבר נמצא בשימוש", "בסדר");
                    await App.Current.MainPage.Navigation.PopAsync();
                }
                else
                {
                    u.Seller = s;
                    User currentS = null;
                    User newS = await proxy.EditProfileAsync(u);
                    currentS = newS;

                    if (currentS == null)
                    {
                        await App.Current.MainPage.DisplayAlert("שגיאה", "שמירת המשתמש נכשלה", "בסדר");
                        //await currentApp.MainPage.Navigation.PopAsync();
                    }
                    else
                    {
                        currentApp.CurrentUser = newS;

                        //if (this.imageFileResult != null)
                        //{
                        //    ServerStatus = "מעלה תמונה...";

                        //    bool success = await proxy.UploadImage(new FileInfo()
                        //    {
                        //        Name = this.imageFileResult.FullPath
                        //    }, $"{newS.Seller.SellerId}.jpg");
                        //}
                        //ServerStatus = "שומר נתונים..."; 
                        currentApp.CurrentUser = newS;
                        await App.Current.MainPage.DisplayAlert("הצלחה", "שמירת המשתמש הצליחה", "בסדר");
                        await currentApp.MainPage.Navigation.PopToRootAsync();
                    }
                }
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("שגיאה", "שמירת המשתמש נכשלה", "בסדר");
                await currentApp.MainPage.Navigation.PopAsync();
            }
        }
        #endregion

        #region ValidateForm
        //This function validate the entire form upon submit!
        private bool ValidateForm()
        {
            //Validate all fields first
            ValidateEmail();
            ValidatePassword();

            //check if any validation failed
            if (ShowEmailError || ShowPasswordError)
                return false;
            return true;
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
