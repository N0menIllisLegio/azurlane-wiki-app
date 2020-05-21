using azurlane_wiki_app.Data;
using azurlane_wiki_app.Data.Tables;
using azurlane_wiki_app.PageShipGirl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using azurlane_wiki_app.Annotations;
using azurlane_wiki_app.PageDownload;
using azurlane_wiki_app.PageEquipmentList;
using azurlane_wiki_app.PageShipGirlList.Items;

namespace azurlane_wiki_app.PageShipGirlList
{
    public class ShipGirlListPageViewModel : INotifyPropertyChanged
    {
        public Object ShipGirlsList => _shipGirlsList.View;
        private CollectionViewSource _shipGirlsList = new CollectionViewSource();

        private string _groupBySelectedItem;

        public ObservableCollection<string> GroupByCollection { get; set; }

        public string GroupBySelectedItem
        {
            get => _groupBySelectedItem;
            set
            {
                _groupBySelectedItem = value;
                _shipGirlsList.GroupDescriptions.Clear();
                _shipGirlsList.GroupDescriptions.Add(new PropertyGroupDescription(value));
            }
        }
        
        private string _sortBySelectedItem;

        public ObservableCollection<string> SortByCollection { get; set; }

        public string SortBySelectedItem
        {
            get => _sortBySelectedItem;
            set
            {
                _sortBySelectedItem = value;

                if (_shipGirlsList.View is ListCollectionView view)
                {
                    if (value == "Name")
                    {
                        view.CustomSort = new ShipGirlNameSorter();
                    }

                    if (value == "ShipID")
                    {
                        view.CustomSort = new ShipGirlIDSorter();
                    }
                    
                    if (value == "Rarity")
                    {
                        view.CustomSort = new ShipGirlRaritySorter();
                    }
                    
                    if (value == "Type")
                    {
                        view.CustomSort = new ShipGirlTypeSorter();
                    }
                }
            }
        }

        private string _filterString;
        private ControlTemplate _displayingContent;

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

        public RelayCommand OpenShipPageCommand { get; set; }
        public RelayCommand OpenGraphicalShipGirlPageCommand { get; set; }
        public RelayCommand OpenTableShipGirlPageCommand { get; set; }
        public RelayCommand OpenEquipmentListPageCommand { get; set; }
        public RelayCommand OpenDownloadPageCommand { get; set; }

        private List<GraphicShipGirlItem> graphList { get; set; }
        private List<TableShipGirlItem> tableList { get; set; }

        public ControlTemplate DisplayingContent
        {
            get => _displayingContent;
            set
            {
                _displayingContent = value;
                OnPropertyChanged(nameof(DisplayingContent));
            }
        }

        public ShipGirlListPageViewModel()
        {
            using (CargoContext cargoContext = new CargoContext())
            {
                List<ShipGirl> shipGirls = cargoContext.ShipGirls.Where(sg => sg.FK_Rarity.Name != "Unreleased").ToList();
                graphList = new List<GraphicShipGirlItem>();
                tableList = new List<TableShipGirlItem>();

                foreach (ShipGirl shipGirl in shipGirls)
                {
                    graphList.Add(new GraphicShipGirlItem(shipGirl));
                    tableList.Add(new TableShipGirlItem(shipGirl));
                }

                OpenShipPageCommand = new RelayCommand(obj =>
                {
                    string id = obj as string;

                    if (!string.IsNullOrEmpty(id))
                    {
                        Navigation.Navigate(new ShipGirlPage(), new ShipGirlPageViewModel(id));
                    }
                });

                GroupByCollection = new ObservableCollection<string> { "Rarity", "Nationality", "Type" };
                SortByCollection = new ObservableCollection<string> { "Name", "ShipID", "Rarity", "Nationality", "Type" };

                OpenGraphicalShipGirlPageCommand = new RelayCommand(obj =>
                {
                    if (_shipGirlsList.Source is List<GraphicShipGirlItem>)
                    {
                        return;
                    }

                    SetGraphicalView();
                });
                
                OpenTableShipGirlPageCommand = new RelayCommand(obj =>
                {
                    if (_shipGirlsList.Source is List<TableShipGirlItem>)
                    {
                       return; 
                    }

                    SetTableView();
                });

                OpenEquipmentListPageCommand = new RelayCommand(obj => 
                {
                    Navigation.Navigate(new EquipmentListPage());
                });

                OpenDownloadPageCommand = new RelayCommand(obj =>
                {
                    Navigation.Navigate(new DownloadPage());
                });

                SetTableView();
            }
        }

        public void SetGraphicalView()
        {
            _shipGirlsList.Source = graphList;
            GroupBySelectedItem = GroupByCollection[0];
            SortBySelectedItem = SortByCollection[0];
            FilterString = "";
            _shipGirlsList.View.Filter = Search;

            DisplayingContent = Application.Current.Resources["GraphicalList"] as ControlTemplate;
        }

        public void SetTableView()
        {
            _shipGirlsList.Source = tableList;

            GroupBySelectedItem = null;
            SortBySelectedItem = null;
            FilterString = "";
            _shipGirlsList.View.Filter = Search;

            DisplayingContent = Application.Current.Resources["TableList"] as ControlTemplate;
        }

        private bool Search(object item)
        {
            BaseShipGirlItem shipGirl = item as BaseShipGirlItem;
            return shipGirl.Name.ToLower().Contains(_filterString.ToLower());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}