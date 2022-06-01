using StoreApp.Models;
using StoreApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace StoreApp.ViewModels
{
    class UploadProductViewModel: BaseViewModel
    {
        Product p;

        #region product name
        private string productName;

        public string ProductName
        {
            get => productName;
            set
            {
                productName = value;
                OnPropertyChanged("ProductName");
            }
        }
        #endregion

        #region  details
        private string details;

        public string Details
        {
            get => details;
            set
            {
                details = value;
                OnPropertyChanged("Details");
            }
        }
        #endregion

        #region surface Material id
        private int sMaterialId;

        public int SMaterialId
        {
            get => sMaterialId;
            set
            {
                sMaterialId = value;
                OnPropertyChanged("SMaterialId");
            }
        }
        #endregion

        #region price
        private double price;

        public double Price
        {
            get => price;
            set
            {
                price = value;
                OnPropertyChanged("Price");
            }
        }
        #endregion

        #region height
        private int height;

        public int Height
        {
            get => height;
            set
            {
                height = value;
                OnPropertyChanged("Height");
            }
        }
        #endregion

        #region width
        private int width;

        public int Width
        {
            get => width;
            set
            {
                width = value;
                OnPropertyChanged("Width");
            }
        }
        #endregion

        #region picture
        private string productImgSrc;

        public string ProductImgSrc
        {
            get => productImgSrc;
            set
            {
                productImgSrc = value;
                OnPropertyChanged("ProductImgSrc");
            }
        }
        private const string DEFAULT_PHOTO_SRC = "defaultphoto.png";
        #endregion

        #region lookup tables

        public List<Models.Color> Colors
        {
            get
            {
                if (((App)App.Current).Tables != null)
                    return ((App)App.Current).Tables.Colors;
                return new List<Models.Color>();
            }
        }
        private Models.Color color;
        public Models.Color Color
        {
            get { return color; }
            set
            {
                color = value;
                OnPropertyChanged("Color");
            }
        }

        public List<Models.Style> Styles
        {
            get
            {
                if (((App)App.Current).Tables != null)
                    return ((App)App.Current).Tables.Styles;
                return new List<Models.Style>();
            }
        }
        private Models.Style style;
        public Models.Style Style
        {
            get { return style; }
            set
            {
                style = value;
                OnPropertyChanged("Style");
            }
        }

        public List<Models.SurfaceMaterial> SurfaceMaterials
        {
            get
            {
                if (((App)App.Current).Tables != null)
                    return ((App)App.Current).Tables.SurfaceMaterials;
                return new List<Models.SurfaceMaterial>();
            }
        }
        private Models.SurfaceMaterial surfaceMaterial;
        public Models.SurfaceMaterial SurfaceMaterial
        {
            get { return surfaceMaterial; }
            set
            {
                surfaceMaterial = value;
                OnPropertyChanged("SurfaceMaterial");
            }
        }

        public List<Models.PaintMaterial> PaintMaterials
        {
            get
            {
                if (((App)App.Current).Tables != null)
                    return ((App)App.Current).Tables.PaintMaterials;
                return new List<Models.PaintMaterial>();
            }
        }
        private Models.PaintMaterial paintMaterial;
        public Models.PaintMaterial PaintMaterial
        {
            get { return paintMaterial; }
            set
            {
                paintMaterial = value;
                OnPropertyChanged("Style");
            }
        }

        #endregion

        #region constructor
        public UploadProductViewModel()
        {
            App theApp = (App)App.Current;
            p = new Product()
            {
                //ProductName = "",
                //Details = "",
                //Price = 0,
                //Height = 0,
                //Width = 0,
                //SMaterial = null,
                //SMaterialId = 0,
                //PMaterial = null,
                //PMaterialId = 0,

                IsActive = true,
                AdvertisingDate = DateTime.Now,
                Seller = currentApp.CurrentUser.Seller,
                SellerId = currentApp.CurrentUser.Seller.SellerId,
            };
            
            //Setup default image photo
            this.ProductImgSrc = StoreAPIProxy.GetImageURL() + DEFAULT_PHOTO_SRC;
            this.imageFileResult = null; //mark that no picture was chosen

            this.SaveDataCommand = new Command(() => SaveData());

            NavigateToPageCommand = new Command<string>(NavigateToPage);
        }
        #endregion

        #region SaveData

        //The command for saving the product
        public ICommand SaveDataCommand { protected set; get; }
        private async void SaveData()
        { 
             
            this.p.ProductName = this.ProductName;
            this.p.Details = this.Details;
            this.p.Price = this.Price;
            this.p.Height = this.Height;
            this.p.Width = this.Width;
            this.p.Color = this.Color;
           
            this.p.Style = this.Style;
           
            this.p.SMaterial = this.SurfaceMaterial;
            
            this.p.PMaterial = this.PaintMaterial;
            
            this.ProductImgSrc = this.ProductImgSrc;
            this.p.Picture = "photogallery.png";
            if (this.productImgSrc == null)
                this.productImgSrc = "photogallery.png";

            ServerStatus = "מתחבר לשרת...";
            await App.Current.MainPage.Navigation.PushModalAsync(new Views.ServerStatusPage(this));
            StoreAPIProxy proxy = StoreAPIProxy.CreateProxy();

            Product currentP = null;
            Product newP = await proxy.UploadProductAsync(this.p);
            currentP = newP;

            if (currentP == null)
            {
                await App.Current.MainPage.DisplayAlert("שגיאה", "פרסום המוצר נכשל", "בסדר");
                await currentApp.MainPage.Navigation.PopModalAsync();
            }
            else
            {
                        
                if (this.imageFileResult != null)
                {
                    ServerStatus = "מעלה תמונה...";

                    bool success = await proxy.UploadImage(new FileInfo()
                    {
                       Name = this.imageFileResult.FullPath
                    }, $"{newP.ProductId}.jpg");
                }
                ServerStatus = "שומר נתונים...";

                await App.Current.MainPage.DisplayAlert("הצלחה", "פרסום המוצר הצליח", "בסדר");
                currentApp.CurrentUser.Seller.Products.Add(p);
               await currentApp.MainPage.Navigation.PopModalAsync();
                await currentApp.MainPage.Navigation.PopAsync();
            }               
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
                this.ProductImgSrc = result.FullPath;
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
                this.ProductImgSrc = result.FullPath;
                if (SetImageSourceEvent != null)
                    SetImageSourceEvent(imgSource);
            }
        }
        #endregion
    }
}
