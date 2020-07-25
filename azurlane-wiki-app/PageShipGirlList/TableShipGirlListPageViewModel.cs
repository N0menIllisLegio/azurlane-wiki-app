using azurlane_wiki_app.Data;
using azurlane_wiki_app.Data.Tables;
using azurlane_wiki_app.PageShipGirlList.Items;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks.Dataflow;
using System.Windows.Data;

namespace azurlane_wiki_app.PageShipGirlList
{
    public class TableShipGirlListPageViewModel : BaseShipGirlListViewModel
    {
        private ObservableCollection<TableShipGirlItem> _tableList;
        private bool statsMaxLevel = true;
        private bool retrofited = false;

        private ObservableCollection<TableShipGirlItem> tableList
        {
            get => _tableList;
            set
            {
                _tableList = value;
                BindingOperations.EnableCollectionSynchronization(_tableList, _personCollectionLock);
            }
        }

        public bool StatsMaxLevel 
        { 
            get => statsMaxLevel;
            set
            {
                if (statsMaxLevel != value)
                {
                    statsMaxLevel = value;
                    OnPropertyChanged(nameof(StatsMaxLevel));
                    InvertStats();
                }
            }
        }

        public bool Retrofited 
        { 
            get => retrofited;
            set
            {            
                retrofited = value;
                OnPropertyChanged(nameof(Retrofited));
                InvertRetrofit();
            }
        }

        public TableShipGirlListPageViewModel() : base()
        {
            tableList = new ObservableCollection<TableShipGirlItem>();

            using (CargoContext cargoContext = new CargoContext())
            {
                List<ShipGirl> shipGirls = cargoContext.
                    ShipGirls.Where(sg => sg.FK_Rarity.Name != "Unreleased").
                    Include(sg => sg.FK_Rarity.FK_Icon).Include(sg => sg.FK_Nationality.FK_Icon).
                    Include(sg => sg.FK_ShipType.FK_Icon).ToList();

                ActionBlock<ShipGirl> loadBlock = new ActionBlock<ShipGirl>(
                    (shipGirl) =>
                    {
                        var tableGirl = new TableShipGirlItem(shipGirl);
                        tableList.Add(tableGirl);
                    });

                foreach (ShipGirl shipGirl in shipGirls)
                {
                    loadBlock.Post(shipGirl);
                }

                loadBlock.Complete();
                loadBlock.Completion.Wait();
            }

            _shipGirlsList.Source = tableList;
        }

        private void InvertRetrofit()
        {
            using (_shipGirlsList.DeferRefresh())
            {
                foreach (var shipGirl in tableList.Where(sg => sg.Remodel))
                {
                    shipGirl.InvertRetrofit();
                }
            }
        }

        private void InvertStats()
        {
            using (_shipGirlsList.DeferRefresh())
            {
                foreach (var shipGirl in tableList)
                {
                    shipGirl.InvertStats();
                }
            }
        }
    }
}
