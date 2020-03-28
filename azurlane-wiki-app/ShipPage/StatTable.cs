using System;
using System.Reflection;
using System.Windows.Media;

namespace azurlane_wiki_app.ShipPage
{
    public struct StatTable
    {
        public StatTable(string tableName, string healthStatValue, string armorStatValue, 
            string reloadStatValue, string luckStatValue, string fireStatValue, string torpStatValue, 
            string evadeStatValue, string speedStatValue, string airStatValue, string consumptionStatValue, 
            string aaStatValue, string accStatValue, string aswStatValue, string oxygenStatValue, string ammoStatValue)
        {
            HealthStatValue = healthStatValue;
            StatTableTitle = tableName;
            ArmorStatValue = armorStatValue;
            ReloadStatValue = reloadStatValue;
            LuckStatValue = luckStatValue;
            FireStatValue = fireStatValue;
            TorpStatValue = torpStatValue;
            EvadeStatValue = evadeStatValue;
            SpeedStatValue = speedStatValue;
            AirStatValue = airStatValue;
            ConsumptionStatValue = consumptionStatValue;
            AAStatValue = aaStatValue;
            AccStatValue = accStatValue;
            ASWStatValue = aswStatValue;
            OxygenStatValue = oxygenStatValue;
            AmmoStatValue = ammoStatValue;

            using (CargoContext cargoContext = new CargoContext())
            {
                HealthStatIcon = cargoContext.Icons.Find("Health")?.FileName;
                ArmorStatIcon = cargoContext.Icons.Find("Armor")?.FileName;
                ReloadStatIcon = cargoContext.Icons.Find("Reload")?.FileName;
                LuckStatIcon = cargoContext.Icons.Find("Luck")?.FileName;
                FireStatIcon = cargoContext.Icons.Find("Firepower")?.FileName;
                TorpStatIcon = cargoContext.Icons.Find("Torpedo")?.FileName;
                EvadeStatIcon = cargoContext.Icons.Find("Evasion")?.FileName;
                AAStatIcon = cargoContext.Icons.Find("Anti-air")?.FileName;
                AirStatIcon = cargoContext.Icons.Find("Aviation")?.FileName;
                ConsumptionStatIcon = cargoContext.Icons.Find("Oil consumption")?.FileName;
                AccStatIcon = cargoContext.Icons.Find("Accuracy")?.FileName;
                ASWStatIcon = cargoContext.Icons.Find("Anti-submarine warfare")?.FileName;
                OxygenStatIcon = cargoContext.Icons.Find("Oxygen")?.FileName;
                AmmoStatIcon = cargoContext.Icons.Find("Ammunition")?.FileName;
                HuntingRangeStatIcon = cargoContext.Icons.Find("Hunting range")?.FileName;
            }

            Width = Height = 25;
        }

        #region HighLight

        public static event EventHandler BC_00Changed;
        public static event EventHandler BC_01Changed;
        public static event EventHandler BC_02Changed;
        public static event EventHandler BC_03Changed;

        public static event EventHandler BC_10Changed;
        public static event EventHandler BC_11Changed;
        public static event EventHandler BC_12Changed;
        public static event EventHandler BC_13Changed;

        public static event EventHandler BC_20Changed;
        public static event EventHandler BC_21Changed;
        public static event EventHandler BC_22Changed;
        public static event EventHandler BC_23Changed;

        public static event EventHandler BC_30Changed;
        public static event EventHandler BC_31Changed;
        public static event EventHandler BC_32Changed;

        private static Brush bc_00 = Brushes.Transparent;
        private static Brush bc_01 = Brushes.Transparent;
        private static Brush bc_02 = Brushes.Transparent;
        private static Brush bc_03 = Brushes.Transparent;

        private static Brush bc_10 = Brushes.Transparent;
        private static Brush bc_11 = Brushes.Transparent;
        private static Brush bc_12 = Brushes.Transparent;
        private static Brush bc_13 = Brushes.Transparent;

        private static Brush bc_20 = Brushes.Transparent;
        private static Brush bc_21 = Brushes.Transparent;
        private static Brush bc_22 = Brushes.Transparent;
        private static Brush bc_23 = Brushes.Transparent;

        private static Brush bc_30 = Brushes.Transparent;
        private static Brush bc_31 = Brushes.Transparent;
        private static Brush bc_32 = Brushes.Transparent;

        // 1
        public static Brush BC_00
        {
            get => bc_00;
            set
            {
                bc_00 = value; BC_00Changed?.Invoke(null, EventArgs.Empty);
            }
        }

        public static Brush BC_01
        {
            get => bc_01;
            set
            {
                bc_01 = value;
                BC_01Changed?.Invoke(null, EventArgs.Empty);
            }
        }

        public static Brush BC_02
        {
            get => bc_02;
            set
            {
                bc_02 = value; BC_02Changed?.Invoke(null, EventArgs.Empty);
            }
        }

        public static Brush BC_03
        {
            get => bc_03;
            set
            {
                bc_03 = value; BC_03Changed?.Invoke(null, EventArgs.Empty);
            }
        }
        
        // 2
        public static Brush BC_10
        {
            get => bc_10;
            set
            {
                bc_10 = value; BC_10Changed?.Invoke(null, EventArgs.Empty);
            }
        }

        public static Brush BC_11
        {
            get => bc_11;
            set
            {
                bc_11 = value;
                BC_11Changed?.Invoke(null, EventArgs.Empty);
            }
        }

        public static Brush BC_12
        {
            get => bc_12;
            set
            {
                bc_12 = value; BC_12Changed?.Invoke(null, EventArgs.Empty);
            }
        }

        public static Brush BC_13
        {
            get => bc_13;
            set
            {
                bc_13 = value; BC_13Changed?.Invoke(null, EventArgs.Empty);
            }
        }
        
        // 3
        public static Brush BC_20
        {
            get => bc_20;
            set
            {
                bc_20 = value; BC_20Changed?.Invoke(null, EventArgs.Empty);
            }
        }

        public static Brush BC_21
        {
            get => bc_21;
            set
            {
                bc_21 = value;
                BC_21Changed?.Invoke(null, EventArgs.Empty);
            }
        }

        public static Brush BC_22
        {
            get => bc_22;
            set
            {
                bc_22 = value; BC_22Changed?.Invoke(null, EventArgs.Empty);
            }
        }

        public static Brush BC_23
        {
            get => bc_23;
            set
            {
                bc_23 = value; BC_23Changed?.Invoke(null, EventArgs.Empty);
            }
        }

        // 4
        public static Brush BC_30
        {
            get => bc_30;
            set
            {
                bc_30 = value; BC_30Changed?.Invoke(null, EventArgs.Empty);
            }
        }

        public static Brush BC_31
        {
            get => bc_31;
            set
            {
                bc_31 = value;
                BC_31Changed?.Invoke(null, EventArgs.Empty);
            }
        }

        public static Brush BC_32
        {
            get => bc_32;
            set
            {
                bc_32 = value; BC_32Changed?.Invoke(null, EventArgs.Empty);
            }
        }

        public static void BrushCell(int row, int column, SolidColorBrush brush)
        {
            string propertyName = "BC_" + row + column;
            PropertyInfo property = typeof(StatTable).GetProperty(propertyName);
            property?.SetValue(property, brush);
        }

        #endregion

        public string StatTableTitle { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        // ICONS
        public string HealthStatIcon { get; set; }
        public string ArmorStatIcon { get; set; }
        public string ReloadStatIcon { get; set; }
        public string LuckStatIcon { get; set; }

        public string FireStatIcon { get; set; }
        public string TorpStatIcon { get; set; }
        public string EvadeStatIcon { get; set; }

        public string AAStatIcon { get; set; }
        public string AirStatIcon { get; set; }
        public string ConsumptionStatIcon { get; set; }
        public string AccStatIcon { get; set; }

        public string ASWStatIcon { get; set; }
        public string OxygenStatIcon { get; set; }
        public string AmmoStatIcon { get; set; }
        public string HuntingRangeStatIcon { get; set; }

        // STATS
        public string HealthStatValue { get; set; }
        public string ArmorStatValue { get; set; }
        public string ReloadStatValue { get; set; }
        public string LuckStatValue { get; set; }

        public string FireStatValue { get; set; }
        public string TorpStatValue { get; set; }
        public string EvadeStatValue { get; set; }
        public string SpeedStatValue { get; set; }

        public string AAStatValue { get; set; }
        public string AirStatValue { get; set; }
        public string ConsumptionStatValue { get; set; }
        public string AccStatValue { get; set; }

        public string ASWStatValue { get; set; }
        public string OxygenStatValue { get; set; }
        public string AmmoStatValue { get; set; }
    }
}