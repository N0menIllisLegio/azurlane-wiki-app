using azurlane_wiki_app.Annotations;
using azurlane_wiki_app.PageEquipment;
using azurlane_wiki_app.PageEquipmentList.Items;
using azurlane_wiki_app.PageShipGirlList;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using azurlane_wiki_app.PageDownload;

namespace azurlane_wiki_app.PageEquipmentList
{
    public class EquipmentListPageViewModel : INotifyPropertyChanged
    {
        private object _equipmentList;

        public Object EquipmentList
        {
            get => _equipmentList;
            set
            {
                _equipmentList = value;
                OnPropertyChanged(nameof(EquipmentList));
            }
        }

        private readonly CollectionViewSource _equipmentListViewSource = new CollectionViewSource();

        public ObservableCollection<string> TypeCollection { get; set; }

        private string _typeSelectedItem;

        public string TypeSelectedItem
        {
            get => _typeSelectedItem;
            set
            {
                _typeSelectedItem = value;
                ChangeEquipmentType(value);
                _equipmentListViewSource.View?.Refresh();
            }
        }

        private string _filterString = "";
        
        public string FilterString
        {
            get => _filterString;
            set
            {
                _filterString = value;
                OnPropertyChanged(nameof(FilterString));
                _equipmentListViewSource.View?.Refresh();
            }
        }

        public RelayCommand OpenEquipmentPageCommand { get; set; }
        public RelayCommand OpenGraphicalShipGirlPageCommand { get; set; }
        public RelayCommand OpenTableShipGirlPageCommand { get; set; }
        public RelayCommand OpenDownloadPageCommand { get; set; }

        private ItemsListsStorage ItemsListsStorage { get; set; } = new ItemsListsStorage();

        public EquipmentListPageViewModel()
        {
            TypeCollection = new ObservableCollection<string>
            {
                "Destroyer Guns",
                "Light Cruiser Guns",
                "Heavy Cruiser Guns",
                "Large Cruiser Guns",
                "Battleship Guns",
                "Ship Torpedoes",
                "Submarine Torpedoes",
                "Fighter Planes",
                "Dive Bomber Planes",
                "Torpedo Bomber Planes",
                "Seaplanes",
                "Anti-Air Guns",
                "Auxiliary Equipment",
                "Anti-Submarine Equipment"
            };

            TypeSelectedItem = TypeCollection[0];
            EquipmentList = _equipmentListViewSource.View;
            _equipmentListViewSource.View.Filter = Search;

            OpenGraphicalShipGirlPageCommand = new RelayCommand(obj =>
            {
                Navigation.Navigate(new GraphicalShipGirlListPage());
            });

            OpenTableShipGirlPageCommand = new RelayCommand(obj =>
            {
                Navigation.Navigate(new TableShipGirlListPage());
            });

            OpenEquipmentPageCommand = new RelayCommand(obj =>
            {
                if (int.TryParse(obj.ToString(), out var id))
                {
                    Navigation.Navigate(new EquipmentPage(), new EquipmentPageViewModel(id));
                }
            });

            OpenDownloadPageCommand = new RelayCommand(obj =>
            {
                Navigation.Navigate(new DownloadPage());
            });
        }

        private void ChangeEquipmentType(string newType)
        {
            ItemsListsStorage.ClearLists();
            ItemsListsStorage.FillList(newType);
            EquipmentList = null;
            _equipmentListViewSource.Source = ItemsListsStorage.GetList(newType);
            EquipmentList = _equipmentListViewSource.View;
            _equipmentListViewSource.View.Filter = Search;
        }

        private bool Search(object item)
        {
            BaseEquipmentItem baseEquipment = item as BaseEquipmentItem;
            return baseEquipment.Name.ToLower().Contains(_filterString.ToLower());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
