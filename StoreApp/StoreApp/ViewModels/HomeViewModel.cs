using StoreApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

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
        private void InitProducts()
        {
            IsRefreshing = true;
            App theApp = (App)App.Current;
            this.allProducts = theApp.CurrentUser.allProducts;


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
                foreach (Product uc in this.allProducts)
                {
                    if (!this.FilteredProducts.Contains(uc))
                        this.FilteredProducts.Add(uc);


                }
            }
            else
            {
                foreach (Product uc in this.allProducts)
                {
                    string contactString = $"{uc.ProductName}|{uc.Details}|{uc.Color}|{uc.Style}";

                    if (!this.FilteredProducts.Contains(uc) &&
                        contactString.Contains(search))
                        this.FilteredProducts.Add(uc);
                    else if (this.FilteredProducts.Contains(uc) &&
                        !contactString.Contains(search))
                        this.FilteredProducts.Remove(uc);
                }
            }

            this.FilteredProducts = new ObservableCollection<Product>(this.FilteredProducts);
        }
        #endregion

    }
}
