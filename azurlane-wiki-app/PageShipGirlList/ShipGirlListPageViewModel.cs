using azurlane_wiki_app.Annotations;
using azurlane_wiki_app.Data;
using azurlane_wiki_app.Data.Tables;
using azurlane_wiki_app.PageDownload;
using azurlane_wiki_app.PageEquipmentList;
using azurlane_wiki_app.PageShipGirl;
using azurlane_wiki_app.PageShipGirlList.Items;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Data.Entity;

namespace azurlane_wiki_app.PageShipGirlList
{
    public class ShipGirlListPageViewModel : INotifyPropertyChanged
    {
        //public Object ShipGirlsList => _shipGirlsList.View;

        public ObservableCollection<GraphicShipGirlItem> GraphicShipGirlsList { get; set; }//= new ObservableCollection<GraphicShipGirlItem>();

        public CollectionViewSource ShipGirlsList => _shipGirlsList;
        private CollectionViewSource _shipGirlsList = new CollectionViewSource();

        private List<GraphicShipGirlItem> graphList { get; set; } = new List<GraphicShipGirlItem>();
        private List<TableShipGirlItem> tableList { get; set; } = new List<TableShipGirlItem>();


        #region Grouping

        private string _groupBySelectedItem;

        public ObservableCollection<string> GroupByCollection { get; set; }
            = new ObservableCollection<string> { "Rarity", "Nationality", "Type" };

        public string GroupBySelectedItem
        {
            get => _groupBySelectedItem;
            set
            {
                _groupBySelectedItem = value;

                using (_shipGirlsList.DeferRefresh())
                {
                    _shipGirlsList.GroupDescriptions.Clear();
                    _shipGirlsList.GroupDescriptions.Add(new PropertyGroupDescription(value));
                }
            }
        }

        #endregion

        #region Sorting

        private string _sortBySelectedItem;

        public ObservableCollection<string> SortByCollection { get; set; } 
            = new ObservableCollection<string> { "Name", "ShipID", "Rarity", "Nationality", "Type" };

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

        #endregion

        #region Filtering

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

        private bool Search(object item)
        {
            BaseShipGirlItem shipGirl = item as BaseShipGirlItem;
            return shipGirl.Name.ToLower().Contains(_filterString.ToLower());
        }

        #endregion

        #region Commands

        public RelayCommand OpenShipPageCommand { get; set; } = new RelayCommand(obj =>
        {
            string id = obj as string;

            if (!string.IsNullOrEmpty(id))
            {
                Navigation.Navigate(new ShipGirlPage(), new ShipGirlPageViewModel(id));
            }
        });

        public RelayCommand OpenEquipmentListPageCommand { get; set; } = new RelayCommand(obj =>
        {
            Navigation.Navigate(new EquipmentListPage());
        });

        public RelayCommand OpenDownloadPageCommand { get; set; } = new RelayCommand(obj =>
        {
            Navigation.Navigate(new DownloadPage());
        });

        public RelayCommand OpenGraphicalShipGirlPageCommand { get; set; }
        public RelayCommand OpenTableShipGirlPageCommand { get; set; }

        #endregion


        // Current template
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

                // Owner...
                List<ShipGirl> shipGirls = cargoContext.
                    ShipGirls.Where(sg => sg.FK_Rarity.Name != "Unreleased").
                    Include(sg => sg.FK_Rarity.FK_Icon).Include(sg => sg.FK_Nationality.FK_Icon).
                    Include(sg => sg.FK_ShipType.FK_Icon).ToList();

                //List<Task<GraphicShipGirlItem>> graphicShips = new List<Task<GraphicShipGirlItem>>();

                foreach (ShipGirl shipGirl in shipGirls)
                {
                    //graphicShips.Add(Task.Factory.StartNew(() => new GraphicShipGirlItem(shipGirl)));

                    graphList.Add(new GraphicShipGirlItem(shipGirl));
                    tableList.Add(new TableShipGirlItem(shipGirl));
                }

                //Task.WaitAll(graphicShips.ToArray());

                //foreach (var task in graphicShips)
                //{
                //    graphList.Add(task.Result);
                //}


            }

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


            SetTableView();
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
            //Application.Current.Dispatcher.Invoke(() =>
            //{
                _shipGirlsList.Source = tableList;
            //});

            GroupBySelectedItem = null;
            SortBySelectedItem = null;
            FilterString = "";
            _shipGirlsList.View.Filter = Search;

            DisplayingContent = Application.Current.Resources["TableList"] as ControlTemplate;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}