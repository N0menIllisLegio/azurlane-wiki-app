using azurlane_wiki_app.Data.Tables;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace azurlane_wiki_app.PageShipGirlList.Items
{
    public class TableShipGirlItem : BaseShipGirlItem
    {
        private BitmapImage shipGirlIcon;

        //public string ShipGirlIcon { get; set; }
        public BitmapImage ShipGirlIcon 
        { 
            get => shipGirlIcon;
            set
            {
                shipGirlIcon = value;
                OnPropertyChanged(nameof(ShipGirlIcon));
            }
        }
        public bool Remodel { get; set; }
        public int Firepower { get; set; }
        public int Health { get; set; }
        public int Evasion { get; set; }
        public int Torpedo { get; set; }
        public int Aviation { get; set; }
        public int AA { get; set; }



        // For sorting on ShipGirlList Table
        public int RaritySorting { get; set; }

        private readonly Dictionary<string, int> Rarities = new Dictionary<string, int>
        {
            { "Normal", 1 },
            { "Rare", 2 },
            { "Elite", 3 },
            { "Super Rare", 4 },
            { "Ultra Rare", 5 },
            { "Priority", 4 },
            { "Decisive", 5 }
        };

        public TableShipGirlItem(ShipGirl shipGirl) : base(shipGirl)
        {
            BitmapImage image;

            try
            {
                image = new BitmapImage(new Uri(@"pack://siteoforigin:,,,"
                                                      + shipGirl.ImageIcon.Remove(0, 1)));
            }
            catch
            {
                image = ImagePathConverter.ImagePlaceholder;
            }

            image.Freeze();

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle,
                    new Action(() => ShipGirlIcon = image));

            // ShipGirlIcon = shipGirl.ImageIcon;

            Remodel = shipGirl.Remodel == "t";
            Aviation = shipGirl.Air120 ?? 0;
            AA = shipGirl.AA120 ?? 0;
            Health = shipGirl.Health120 ?? 0;
            Evasion = shipGirl.Evade120 ?? 0;
            Torpedo = shipGirl.Torp120 ?? 0;
            Firepower = shipGirl.Fire120 ?? 0;

            if (Rarities.ContainsKey(Rarity))
            {
                RaritySorting = Rarities[Rarity];
            }
            else
            {
                RaritySorting = 0;
            }
        }
    }
}
