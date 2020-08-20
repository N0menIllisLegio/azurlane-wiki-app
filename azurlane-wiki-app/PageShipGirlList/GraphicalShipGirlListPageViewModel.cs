using azurlane_wiki_app.Data;
using azurlane_wiki_app.Data.Tables;
using azurlane_wiki_app.PageShipGirlList.Items;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks.Dataflow;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace azurlane_wiki_app.PageShipGirlList
{
    public class GraphicalShipGirlListPageViewModel : BaseShipGirlListViewModel
    {

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

                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    using (_shipGirlsList.DeferRefresh())
                    {
                        _shipGirlsList.GroupDescriptions.Clear();
                        _shipGirlsList.GroupDescriptions.Add(new PropertyGroupDescription(_groupBySelectedItem));
                    }
                }));
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

        #region Retrofit

        private bool retrofited = false;
        public bool Retrofited
        {
            get => retrofited;
            set
            {
                retrofited = value;
                OnPropertyChanged(nameof(Retrofited));
                InvertRetrofit();
                _shipGirlsList.View.Refresh();
            }
        }

        #endregion

        private ObservableCollection<GraphicShipGirlItem> _graphList;
        private ObservableCollection<GraphicShipGirlItem> graphList 
        { 
            get => _graphList; 
            set 
            {
                _graphList = value;
                BindingOperations.EnableCollectionSynchronization(_graphList, _personCollectionLock);
            } 
        }

        public GraphicalShipGirlListPageViewModel() : base()
        {
            graphList = new ObservableCollection<GraphicShipGirlItem>();

            using (CargoContext cargoContext = new CargoContext())
            {
                var lists = GetRaritiesLists();

                List<ShipGirl> shipGirls = cargoContext.
                    ShipGirls.Where(sg => sg.FK_Rarity.Name != "Unreleased").
                    Include(sg => sg.FK_Rarity.FK_Icon).Include(sg => sg.FK_Nationality.FK_Icon).
                    Include(sg => sg.FK_ShipType.FK_Icon).ToList();

                ActionBlock<ShipGirl> loadBlock = new ActionBlock<ShipGirl>(
                    (shipGirl) => 
                    {
                        GraphicShipGirlItem graphGirl;

                        if (shipGirl.Remodel == "t")
                        {
                            int currentRarityIndex = lists.rarityNames.IndexOf(shipGirl.Rarity);

                            string retrofittedRarityName = lists.rarityNames[currentRarityIndex + 1];
                            string retrofittedRarityIcon = lists.rarityIconsPaths[currentRarityIndex + 1];

                            graphGirl = new GraphicShipGirlItem(shipGirl, retrofittedRarityName, retrofittedRarityIcon);
                        }
                        else
                        {
                            graphGirl = new GraphicShipGirlItem(shipGirl);
                        }
                        
                        graphList.Add(graphGirl);
                    });

                using (_shipGirlsList.DeferRefresh())
                {
                    foreach (ShipGirl shipGirl in shipGirls)
                    {
                        loadBlock.Post(shipGirl);
                    }

                    loadBlock.Complete();
                    loadBlock.Completion.Wait();
                }
            }

            _shipGirlsList.Source = graphList;
            GroupBySelectedItem = GroupByCollection[0];
            SortBySelectedItem = SortByCollection[0];
        }

        private void InvertRetrofit()
        { 
            using (_shipGirlsList.DeferRefresh())
            {
                foreach (var shipGirl in graphList.Where(sg => sg.Remodel))
                {
                    shipGirl.InvertRetrofit();
                }
            }
        }
    }
}
