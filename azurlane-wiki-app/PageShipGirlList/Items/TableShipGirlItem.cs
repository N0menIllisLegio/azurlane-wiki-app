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
        private int firepower;
        private int health;
        private int evasion;
        private int torpedo;
        private int aviation;
        private int aA;

        // Stores init or max values currently displayed        
        private int[] dispBuffer = new int[6];

        // Stores init and max values that is not currently displayed
        // for ships with retrofit
        private BitmapImage bufShipGirlIcon;
        private int[] nonDispBufferMain = new int[6];
        private int[] nonDispBufferAdditional = new int[6];

        public BitmapImage ShipGirlIcon
        {
            get => shipGirlIcon;
            set
            {
                shipGirlIcon = value;
                OnPropertyChanged(nameof(ShipGirlIcon));
            }
        }        
        public int Firepower 
        { 
            get => firepower;
            set
            {
                firepower = value;
                OnPropertyChanged(nameof(Firepower));
            }
        }
        public int Health 
        { 
            get => health;
            set
            {
                health = value;
                OnPropertyChanged(nameof(Health));
            }
        }
        public int Evasion 
        { 
            get => evasion;
            set
            {
                evasion = value;
                OnPropertyChanged(nameof(Evasion));
            }
        }
        public int Torpedo 
        { 
            get => torpedo;
            set
            {
                torpedo = value;
                OnPropertyChanged(nameof(Torpedo));
            }
        }
        public int Aviation 
        { 
            get => aviation;
            set
            {
                aviation = value;
                OnPropertyChanged(nameof(Aviation));
            }
        }
        public int AA 
        { 
            get => aA;
            set
            {
                aA = value;
                OnPropertyChanged(nameof(AA));
            }
        }

        // For sorting on ShipGirlList Table
        public int RaritySorting { get; set; }

        public TableShipGirlItem(ShipGirl shipGirl, string name = null, string iconPath = null) : base(shipGirl, name, iconPath)
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
                Logger.Write($"Failed to load image. path: {shipGirl.ImageIcon}", this.GetType().ToString());
            }

            image.Freeze();

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle,
                    new Action(() => ShipGirlIcon = image));
         

            Aviation = shipGirl.Air120 ?? 0;
            AA = shipGirl.AA120 ?? 0;
            Health = shipGirl.Health120 ?? 0;
            Evasion = shipGirl.Evade120 ?? 0;
            Torpedo = shipGirl.Torp120 ?? 0;
            Firepower = shipGirl.Fire120 ?? 0;

            dispBuffer[0] = shipGirl.AirInitial ?? 0;
            dispBuffer[1] = shipGirl.AAInitial ?? 0;
            dispBuffer[2] = shipGirl.HealthInitial ?? 0;
            dispBuffer[3] = shipGirl.EvadeInitial ?? 0;
            dispBuffer[4] = shipGirl.TorpInitial ?? 0;
            dispBuffer[5] = shipGirl.FireInitial ?? 0;

            if (Remodel)
            {
                BitmapImage imageKai;

                try
                {
                    imageKai = new BitmapImage(new Uri(@"pack://siteoforigin:,,,"
                                                          + shipGirl.ImageIconKai.Remove(0, 1)));
                }
                catch
                {
                    imageKai = ImagePathConverter.ImagePlaceholder;
                    Logger.Write($"Failed to load image. path: {shipGirl.ImageIconKai}", this.GetType().ToString());
                }

                imageKai.Freeze();

                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle,
                        new Action(() => bufShipGirlIcon = imageKai));

                nonDispBufferMain[0] = shipGirl.AirKai120 ?? 0;
                nonDispBufferMain[1] = shipGirl.AAKai120 ?? 0;
                nonDispBufferMain[2] = shipGirl.HealthKai120 ?? 0;
                nonDispBufferMain[3] = shipGirl.EvadeKai120 ?? 0;
                nonDispBufferMain[4] = shipGirl.TorpKai120 ?? 0;
                nonDispBufferMain[5] = shipGirl.FireKai120 ?? 0;

                nonDispBufferAdditional[0] = shipGirl.AirKai ?? 0;
                nonDispBufferAdditional[1] = shipGirl.AAKai ?? 0;
                nonDispBufferAdditional[2] = shipGirl.HealthKai ?? 0;
                nonDispBufferAdditional[3] = shipGirl.EvadeKai ?? 0;
                nonDispBufferAdditional[4] = shipGirl.TorpKai ?? 0;
                nonDispBufferAdditional[5] = shipGirl.FireKai ?? 0;
            }

            SetSortPriority();
        }

        public override void InvertRetrofit()
        {
            base.InvertRetrofit();

            if (Remodel)
            {
                SetSortPriority();

                ShipGirlIcon = Swap(ShipGirlIcon, ref bufShipGirlIcon);

                Aviation = Swap(Aviation, ref nonDispBufferMain[0]);
                AA = Swap(AA, ref nonDispBufferMain[1]);
                Health = Swap(Health, ref nonDispBufferMain[2]);
                Evasion = Swap(Evasion, ref nonDispBufferMain[3]);
                Torpedo = Swap(Torpedo, ref nonDispBufferMain[4]);
                Firepower = Swap(Firepower, ref nonDispBufferMain[5]);

                var temp = nonDispBufferAdditional;
                nonDispBufferAdditional = dispBuffer;
                dispBuffer = temp;
            }
        }

        public void InvertStats()
        {
            // Push currently displayed data in buffer
            // and displays data wich previously was in bufer
            Aviation = Swap(Aviation, ref dispBuffer[0]);
            AA = Swap(AA, ref dispBuffer[1]);
            Health = Swap(Health, ref dispBuffer[2]);
            Evasion = Swap(Evasion, ref dispBuffer[3]);
            Torpedo = Swap(Torpedo, ref dispBuffer[4]);
            Firepower = Swap(Firepower, ref dispBuffer[5]);

            if (Remodel)
            {
                var temp = nonDispBufferMain;
                nonDispBufferMain = nonDispBufferAdditional;
                nonDispBufferMain = temp;
            }
        }

        private void SetSortPriority()
        {
            Dictionary<string, int> Rarities = new Dictionary<string, int>
            {
                { "Normal", 1 },
                { "Rare", 2 },
                { "Elite", 3 },
                { "Super Rare", 4 },
                { "Ultra Rare", 5 },
                { "Priority", 4 },
                { "Decisive", 5 }
            };

            // SORT
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
