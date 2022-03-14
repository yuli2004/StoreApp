﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using StoreApp.Models;
using StoreApp.Services;
using StoreApp.Views;

namespace StoreApp.ViewModels
{
    class HomeViewModel: BaseViewModel
    {

        #region product lists
        public List<Models.Product> allProducts { get; set; }
        
        private ObservableCollection<Models.Product> filteredProducts;
        public ObservableCollection<Models.Product> FilteredProducts 
        {
            get
            {
                return this.filteredProducts;
            }
            set
            {
                if (this.filteredProducts != value)
                {

                    this.filteredProducts = value;
                    OnPropertyChanged("FilteredProducts");
                }
            }
        }

        #endregion
        #region Selected Product
        private Models.Product selectedProduct;
        public Models.Product SelectedProduct
        {
            get => selectedProduct;
            set
            {
                if (value != selectedProduct)
                {
                    selectedProduct = value;
                    OnPropertyChanged("SelectedProduct");
                }
            }
        }
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
            SliderValue = 0;

            this.SearchTerm = String.Empty;
            FilteredProducts = new ObservableCollection<Models.Product>(((App)App.Current).Tables.AllProducts);
            InitProducts();
            SearchProductCommand = new Command<string>(OnTextChanged);
            OnSelectedProduct = new Command<Models.Product>(MoveToProductPage);
        }

        private async void MoveToProductPage(Models.Product obj)
        {
            if (SelectedProduct != null)
            {
                var page = new Views.Product() { Title = "Product Page" };
                var binding = new ProductViewModel() { P = obj };
                page.BindingContext = binding;
                await this.currentApp.MainPage.Navigation.PushAsync(page);
                SelectedProduct = null;
            }


            
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

        private int sliderValue;
        public int SliderValue
        {
            get { return sliderValue; }
            set
            {
                sliderValue = value;
                OnPropertyChanged("SliderValue");
            }
        }

        private void InitProducts()
        {
            IsRefreshing = true;
            App theApp = (App)App.Current;
            this.allProducts = theApp.Tables.AllProducts;

            //Copy list to the filtered list
            this.FilteredProducts = new ObservableCollection<Models.Product>(this.allProducts);
            SearchTerm = String.Empty;
            IsRefreshing = false;
          
        }

        #region Search
        public ICommand SearchProductCommand { get; protected set; }
        public void OnTextChanged(string search)
        {
            //Filter the list of contacts based on the search term
            if (this.allProducts == null)
                return;
            if (String.IsNullOrWhiteSpace(search) || String.IsNullOrEmpty(search))
            {
                foreach (Models.Product pr in this.allProducts)
                {
                    if (!this.FilteredProducts.Contains(pr))                
                        this.FilteredProducts.Add(pr);
                }
            }
            else
            {
                foreach (Models.Product pr in this.allProducts)
                {
                    string contactString = $"{pr.ProductName}|{pr.Details}|{pr.Color}|{pr.Style}|{pr.SMaterial}|{pr.PMaterialId}";

                    if (!this.FilteredProducts.Contains(pr) &&
                        contactString.Contains(search))
                        this.FilteredProducts.Add(pr);
                    else if (this.FilteredProducts.Contains(pr) &&
                        !contactString.Contains(search))
                        this.FilteredProducts.Remove(pr);
                }
            }

            this.FilteredProducts = new ObservableCollection<Models.Product>(this.FilteredProducts);
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

        #region picker commend

        public ICommand PickerCommand => new Command(SearchProduct);

        public async void SearchProduct()       
        {
            
           
            FilteredProducts.Clear();

           
            foreach (Models.Product pr in allProducts)
            {
                //if (sliderValue == 0 || sliderValue > pr.Price)
                //{
                //    page.FilteredProducts.Add(pr);
                //    added = true;
                //}

                if ((Color == null || Color.ColorId == pr.ColorId) 
                    &&(Style == null || Style.StyleId == pr.StyleId)
                    &&(SurfaceMaterial == null || SurfaceMaterial.SMaterialId == pr.SMaterialId) 
                    && (PaintMaterial == null || PaintMaterial.PMaterialId == pr.PMaterialId)
                    )
                {
                   FilteredProducts.Add(pr);
                  

                }

            }

            //Page p = new Home();
            //p.BindingContext = page;
            //await App.Current.MainPage.Navigation.PushModalAsync(p);
        }
        #endregion

        #region open product page

       public ICommand OnSelectedProduct { get;  protected set; }

        #endregion
    }
}
