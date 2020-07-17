using azurlane_wiki_app.Annotations;
using azurlane_wiki_app.PageDownload;
using azurlane_wiki_app.PageEquipmentList;
using azurlane_wiki_app.PageShipGirl;
using azurlane_wiki_app.PageShipGirlList.Items;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;

namespace azurlane_wiki_app.PageShipGirlList
{
    public abstract class BaseShipGirlListViewModel : INotifyPropertyChanged
    {
        #region NotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Filtering

        private string _filterString;

        public string FilterString
        {
            get => _filterString;
            set
            {
                _filterString = value;
                OnPropertyChanged(nameof(FilterString));
                _shipGirlsList.View.Refresh();
            }
        }

        protected bool Search(object item)
        {
            BaseShipGirlItem shipGirl = item as BaseShipGirlItem;
            return shipGirl.Name.ToLower().Contains(_filterString.ToLower());
        }

        #endregion

        #region Commands

        public RelayCommand OpenShipPageCommand { get; set; }
        public RelayCommand OpenEquipmentListPageCommand { get; set; } 
        public RelayCommand OpenDownloadPageCommand { get; set; }
        public RelayCommand OpenGraphicalShipGirlPageCommand { get; set; } 
        public RelayCommand OpenTableShipGirlPageCommand { get; set; }

        #endregion

        public Object ShipGirlsList => _shipGirlsList.View;

        public CollectionViewSource _shipGirlsList = new CollectionViewSource();

        protected readonly object _personCollectionLock;

        public BaseShipGirlListViewModel()
        {
            _personCollectionLock = new object();

            OpenTableShipGirlPageCommand = new RelayCommand(obj =>
            {
                Navigation.Navigate(new TableShipGirlListPage());
            });

            OpenGraphicalShipGirlPageCommand = new RelayCommand(obj =>
            {
                Navigation.Navigate(new GraphicalShipGirlListPage());
            });

            OpenDownloadPageCommand = new RelayCommand(obj =>
            {
                Navigation.Navigate(new DownloadPage());
            });

            OpenEquipmentListPageCommand = new RelayCommand(obj =>
            {
                Navigation.Navigate(new EquipmentListPage());
            });

            OpenShipPageCommand = new RelayCommand(obj =>
            {
                string id = obj as string;

                if (!string.IsNullOrEmpty(id))
                {
                    Navigation.Navigate(new ShipGirlPage(), new ShipGirlPageViewModel(id));
                }
            });
        }
    }
}
