using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using StoreApp.Models;


namespace StoreApp.ViewModels
{
    class HomeViewModel: BaseViewModel
    {

        #region product lists
        public List<Product> allProducts { get; set; }
       // private ObservableCollection<Product> filteredProducts;
        public ObservableCollection<Product> FilteredProducts { get; set; }
       
        #endregion

        #region search term
        private string searchTerm;
        public string SearchTerm
        {
            get
            {
                return this.searchTerm;
            }
            set
            {
                if (this.searchTerm != value)
                {

                    this.searchTerm = value;
                    OnTextChanged(value);
                    OnPropertyChanged("SearchTerm");
                }
            }
        }
        #endregion

        #region constructor
        public HomeViewModel()
        {
            this.SearchTerm = String.Empty;
            FilteredProducts = new ObservableCollection<Product>(((App)App.Current).Tables.AllProducts);
            InitProducts();
        }
        #endregion

        #region Refresh
        private bool isRefreshing;
        public bool IsRefreshing
        {
            get => isRefreshing;
            set
            {
                if (this.isRefreshing != value)
                {
                    this.isRefreshing = value;
                    OnPropertyChanged(nameof(IsRefreshing));
                }
            }
        }
        public ICommand RefreshCommand => new Command(OnRefresh);
        public void OnRefresh()
        {
            InitProducts();
        }
        #endregion

        private void InitProducts()
        {
            IsRefreshing = true;
            App theApp = (App)App.Current;
            this.allProducts = theApp.Tables.AllProducts;

            //Copy list to the filtered list
            this.FilteredProducts = new ObservableCollection<Product>(this.allProducts);
            SearchTerm = String.Empty;
            IsRefreshing = false;
        }

        #region Search
        public void OnTextChanged(string search)
        {
            //Filter the list of contacts based on the search term
            if (this.allProducts == null)
                return;
            if (String.IsNullOrWhiteSpace(search) || String.IsNullOrEmpty(search))
            {
                foreach (Product p in this.allProducts)
                {
                    if (!this.FilteredProducts.Contains(p))
                        this.FilteredProducts.Add(p);
                }
            }
            else
            {
                foreach (Product p in this.allProducts)
                {
                    string contactString = $"{p.ProductName}|{p.Details}|{p.Color}|{p.Style}|{p.SMaterial}|{p.PMaterialId}";

                    if (!this.FilteredProducts.Contains(p) &&
                        contactString.Contains(search))
                        this.FilteredProducts.Add(p);
                    else if (this.FilteredProducts.Contains(p) &&
                        !contactString.Contains(search))
                        this.FilteredProducts.Remove(p);
                }
            }

            this.FilteredProducts = new ObservableCollection<Product>(this.FilteredProducts);
        }
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

        public List<Models.SurfaceMaterial> SMaterials
        {
            get
            {
                if (((App)App.Current).Tables != null)
                    return ((App)App.Current).Tables.SurfaceMaterials;
                return new List<Models.SurfaceMaterial>();
            }
        }
        private Models.SurfaceMaterial sMaterial;
        public Models.SurfaceMaterial SMaterial
        {
            get { return sMaterial; }
            set
            {
                sMaterial = value;
                OnPropertyChanged("SMaterial");
            }
        }

        public List<Models.PaintMaterial> PMaterials
        {
            get
            {
                if (((App)App.Current).Tables != null)
                    return ((App)App.Current).Tables.PaintMaterials;
                return new List<Models.PaintMaterial>();
            }
        }
        private Models.PaintMaterial pMaterial;
        public Models.PaintMaterial PMaterial
        {
            get { return pMaterial; }
            set
            {
                pMaterial = value;
                OnPropertyChanged("Style");
            }
        }

        #endregion

        #region picker commend
        public HomeViewModel()
        {
            SliderValue = 0;
        }

        public ICommand SearchCommand => new Command(SearchInstructor);

        public async void SearchInstructor()
        {
            LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
            instructors = await proxy.GetAllInstructorsAsync();
            HomePageViewModel page = new HomePageViewModel();
            bool added = false;

            foreach (Instructor i in instructors)
            {
                if (sliderValue == 0 || sliderValue == i.Price)
                {
                    page.InstructorList.Add(i);
                    added = true;
                }

                if (Area != null && Area.AreaId != i.AreaId)
                {
                    if (added)
                    {
                        page.InstructorList.Remove(i);
                        added = false;
                    }
                }

                if (Gender != null && Gender.GenderId != i.GenderId)
                {
                    if (added)
                    {
                        page.InstructorList.Remove(i);
                        added = false;
                    }
                }

                if (Gearbox != null && Gearbox.GearboxId != i.GearboxId)
                {
                    if (added)
                    {
                        page.InstructorList.Remove(i);
                        added = false;
                    }
                }

                if (LicenseType != null && LicenseType.LicenseTypeId != i.LicenseTypeId)
                {
                    if (added)
                    {
                        page.InstructorList.Remove(i);
                        added = false;
                    }
                }
            }

            Page p = new HomePageView();
            p.BindingContext = page;
            await App.Current.MainPage.Navigation.PushModalAsync(p);
        }
        #endregion
    }
}
