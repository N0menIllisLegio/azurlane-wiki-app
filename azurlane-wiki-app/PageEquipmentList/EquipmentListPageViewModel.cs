using azurlane_wiki_app.Properties;
using azurlane_wiki_app.PageDownload;
using azurlane_wiki_app.PageEquipment;
using azurlane_wiki_app.PageEquipmentList.Items;
using azurlane_wiki_app.PageShipGirlList;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;

namespace azurlane_wiki_app.PageEquipmentList
{
    public class EquipmentListPageViewModel : INotifyPropertyChanged
    {
        private object _equipmentList;
        private string _typeSelectedItem;
        private string _filterString = "";
        private bool _maxStats = true;
        private bool _maxRarity = false;
        private readonly CollectionViewSource _equipmentListViewSource = new CollectionViewSource();
        private ItemsListsStorage ItemsListsStorage = new ItemsListsStorage();

        #region Bindings

        public Object EquipmentList
        {
            get => _equipmentList;
            set
            {
                _equipmentList = value;
                OnPropertyChanged(nameof(EquipmentList));
            }
        }

        public ObservableCollection<string> TypeCollection { get; set; }

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

        public bool MaxStats
        {
            get => _maxStats;
            set
            {
                if (value != _maxStats)
                {
                    _maxStats = value;
                    OnPropertyChanged(nameof(MaxStats));
                    ItemsListsStorage.ChangeCurrentListStats(TypeSelectedItem);
                }
            }
        }

        public bool MaxRarity
        {
            get => _maxRarity;
            set
            {
                if (value != _maxRarity)
                {
                    _maxRarity = value;
                    OnPropertyChanged(nameof(MaxRarity));
                    _equipmentListViewSource.View?.Refresh();
                }
            }
        }

        

        public RelayCommand OpenEquipmentPageCommand { get; set; }
        public RelayCommand OpenGraphicalShipGirlPageCommand { get; set; }
        public RelayCommand OpenTableShipGirlPageCommand { get; set; }
        public RelayCommand OpenDownloadPageCommand { get; set; }

        #endregion

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

            if (!MaxStats)
            {
                ItemsListsStorage.ChangeCurrentListStats(TypeSelectedItem);
            }
        }

        private bool Search(object item)
        {
            BaseEquipmentItem baseEquipment = item as BaseEquipmentItem;
            return baseEquipment.Name.ToLower().Contains(_filterString.ToLower()) && (baseEquipment.IsMaxRarity || !MaxRarity);
        }



        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
