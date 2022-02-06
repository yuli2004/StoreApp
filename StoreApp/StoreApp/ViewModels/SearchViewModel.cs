using StoreApp.Models;
using StoreApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace StoreApp.ViewModels
{
    public class SearchViewModel: BaseViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand PerformSearch => new Command<string>(async (string query) =>
        {
            StoreAPIProxy proxy = StoreAPIProxy.CreateProxy();
            List<Product> result = await proxy.GetSearchResults(query);
            SearchResults = new ObservableCollection<Product>(result);
        });

        private ObservableCollection<Product> searchResults;
        public ObservableCollection<Product> SearchResults
        {
            get
            {
                return searchResults;
            }
            set
            {
                searchResults = value;
                NotifyPropertyChanged("SearchResults");
            }
        }
    }
}
