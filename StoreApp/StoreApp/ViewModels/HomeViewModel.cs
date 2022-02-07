using StoreApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace StoreApp.ViewModels
{
    class HomeViewModel: BaseViewModel
    {

        #region Products lists
        private List<Product> allProducts;
        private ObservableCollection<Product> filteredProducts;
        public ObservableCollection<Product> FilteredProducts
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
            this.allProducts = theApp.CurrentUser.AllProducts;

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
                    string contactString = $"{p.ProductName}|{p.Details}|{p.Color}|{p.Style}|{p.Material}";

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


    }
}
