using azurlane_wiki_app.Data.Tables;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace azurlane_wiki_app
{
    public struct GeneralInfoItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public int IconWidth { get; set; }
        public int IconHeight { get; set; }
    }

    public struct EquipmentItem
    {
        public string Slot { get; set; }
        public string EfficiencyInit { get; set; }
        public string EfficiencyMax { get; set; }
        public string EfficiencyKai { get; set; }
        public string Equippable { get; set; }
    }

    public class DropItem
    {
        public string Map { get; set; }
        public PackIconKind Ch_1 { get; set; } = PackIconKind.Close;
        public PackIconKind Ch_2 { get; set; } = PackIconKind.Close;
        public PackIconKind Ch_3 { get; set; } = PackIconKind.Close;
        public PackIconKind Ch_4 { get; set; } = PackIconKind.Close;
        public PackIconKind Ch_5 { get; set; } = PackIconKind.Close;
        public PackIconKind Ch_6 { get; set; } = PackIconKind.Close;
        public PackIconKind Ch_7 { get; set; } = PackIconKind.Close;
        public PackIconKind Ch_8 { get; set; } = PackIconKind.Close;
        public PackIconKind Ch_9 { get; set; } = PackIconKind.Close;
        public PackIconKind Ch_10 { get; set; } = PackIconKind.Close;
        public PackIconKind Ch_11 { get; set; } = PackIconKind.Close;
        public PackIconKind Ch_12 { get; set; } = PackIconKind.Close;
        public PackIconKind Ch_13 { get; set; } = PackIconKind.Close;
    }

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

    public class ShipGirlPageViewModel
    {
        public string ShipName { get; set; }
        public string ShipGirlIcon { get; set; }

        public string ChibiImage { get; set; }
        public string BannerImage { get; set; }
        public string MainImage { get; set; }
        public List<GeneralInfoItem> GeneralInfoList { get; set; }
        public string ShipGirlTypeIcon { get; set; }
        public string ShipGirlSubtypeIcon { get; set; }
        public string ShipGirlType { get; set; }
        public string ShipGirlSubtype { get; set; }
        public string CoinIcon { get; set; }
        public string OilIcon { get; set; }
        public string MedalIcon { get; set; }
        public string ScrapCoinsValue { get; set; }
        public string ScrapOilValue { get; set; }
        public string ScrapMedalsValue { get; set; }

        public StatTable InitialStats { get; set; }
        public StatTable Level100Stats { get; set; }
        public StatTable Level120Stats { get; set; }

        public List<EquipmentItem> EquipmentList { get; set; }

        public IEnumerable<Skill> SkillsList { get; set; }

        public string Rarity { get; set; }

        public Visibility RetrofitVisibility { get; set; } = Visibility.Visible;

        #region LimitBreakProperties
        public string LimitBreak1 { get; set; }
        public string LimitBreak2 { get; set; }
        public string LimitBreak3 { get; set; }

        #endregion

        #region DropProperties

        public List<DropItem> DropsList { get; set; }
        public PackIconKind LightConstructionIcon { get; set; } = PackIconKind.Close;
        public PackIconKind HeavyConstructionIcon { get; set; } = PackIconKind.Close;
        public PackIconKind AviationConstructionIcon { get; set; } = PackIconKind.Close;
        public PackIconKind LimitedConstructionIcon { get; set; } = PackIconKind.Close;
        public PackIconKind ExchangeConstructionIcon { get; set; } = PackIconKind.Close;
        public string ConstructionNote { get; set; } = "";
        public string ConstructionTime { get; set; }

        #endregion

        public ShipGirlPageViewModel(ShipGirl shipGirl)
        {
            ShipName = shipGirl.Name;
            ShipGirlIcon = shipGirl.ImageIcon;
            MainImage = shipGirl.Image;

            ChibiImage = shipGirl.ImageChibi;
            BannerImage = shipGirl.ImageBanner;

            GeneralInfoList = new List<GeneralInfoItem>
            {
                new GeneralInfoItem {Name = "ID", Description = shipGirl.ShipID},
                new GeneralInfoItem {Name = "Class", Description = shipGirl.FK_ShipClass.Name},
                new GeneralInfoItem {Name = "Construction Time", Description = shipGirl.ConstructTime},
                new GeneralInfoItem
                {
                    Name = "Nationality", 
                    Description = shipGirl.FK_Nationality.Name,
                    Icon = shipGirl.FK_Nationality.FK_Icon.FileName,
                    IconHeight = 52,
                    IconWidth = 50
                },
                new GeneralInfoItem
                {
                    Name = "Rarity", 
                    Description = shipGirl.FK_Rarity.Name,
                    Icon = shipGirl.FK_Rarity.FK_Icon.FileName,
                    IconHeight = 25,
                    IconWidth = 50
                }
            };

            Rarity = shipGirl.FK_Rarity.Name;
            ShipGirlTypeIcon = shipGirl.FK_ShipType.FK_Icon.FileName;
            ShipGirlType = shipGirl.FK_ShipType.Name;

            if (shipGirl.FK_SubtypeRetro != null)
            {
                ShipGirlSubtypeIcon = shipGirl.FK_SubtypeRetro.FK_Icon.FileName;
                ShipGirlSubtype = shipGirl.FK_SubtypeRetro.Name;
            }


            if (shipGirl.FK_Rarity.Name != "Priority" 
                && shipGirl.FK_Rarity.Name != "Decisive" 
                && shipGirl.FK_Rarity.Name != "Unreleased")
            {
                ScrapCoinsValue = shipGirl.FK_Rarity.ScrapCoins.ToString();
                ScrapOilValue = shipGirl.FK_Rarity.ScrapOil.ToString();
                ScrapMedalsValue = shipGirl.FK_Rarity.ScrapMedals.ToString();

                using (CargoContext cargoContext = new CargoContext())
                {
                    CoinIcon = cargoContext.Icons.Find("Coin")?.FileName;
                    OilIcon = cargoContext.Icons.Find("Oil")?.FileName;
                    MedalIcon = cargoContext.Icons.Find("Medal")?.FileName;
                }
            }
            else
            {
                ScrapCoinsValue = "Cannot be scrapped";
            }
            
            InitialStats = new StatTable("Base Stats", shipGirl.HealthInitial.ToString(), 
                shipGirl.Armor, shipGirl.ReloadInitial.ToString(), shipGirl.Luck.ToString(), 
                shipGirl.FireInitial.ToString(), shipGirl.TorpInitial.ToString(), shipGirl.EvadeInitial.ToString(), 
                shipGirl.Speed.ToString(), shipGirl.AirInitial.ToString(), shipGirl.ConsumptionInitial.ToString(), 
                shipGirl.AAInitial.ToString(), shipGirl.AccInitial.ToString(), shipGirl.ASWInitial.ToString(),
                shipGirl.OxygenInitial.ToString(), shipGirl.AmmoInitial.ToString());

            Level100Stats = new StatTable("Level 100", shipGirl.Health120.ToString(),
                shipGirl.Armor, shipGirl.Reload120.ToString(), shipGirl.Luck.ToString(),
                shipGirl.Fire120.ToString(), shipGirl.Torp120.ToString(), shipGirl.Evade120.ToString(),
                shipGirl.Speed.ToString(), shipGirl.Air120.ToString(), shipGirl.Consumption120.ToString(),
                shipGirl.AA120.ToString(), shipGirl.Acc120.ToString(), shipGirl.ASW120.ToString(),
                shipGirl.Oxygen120.ToString(), shipGirl.Ammo120.ToString());

            Level120Stats = new StatTable("Level 120", shipGirl.HealthMax.ToString(),
                shipGirl.Armor, shipGirl.ReloadMax.ToString(), shipGirl.Luck.ToString(),
                shipGirl.FireMax.ToString(), shipGirl.TorpMax.ToString(), shipGirl.EvadeMax.ToString(),
                shipGirl.Speed.ToString(), shipGirl.AirMax.ToString(), shipGirl.ConsumptionMax.ToString(),
                shipGirl.AAMax.ToString(), shipGirl.AccMax.ToString(), shipGirl.ASWMax.ToString(),
                shipGirl.OxygenMax.ToString(), shipGirl.AmmoMax.ToString());

            EquipmentList = new List<EquipmentItem>
            {
                new EquipmentItem
                {
                    Slot = "1",
                    EfficiencyInit = shipGirl.Eq1EffInit,
                    EfficiencyMax = shipGirl.Eq1EffInitMax,
                    EfficiencyKai = shipGirl.Eq1EffInitKai,
                    Equippable = shipGirl.FK_Eq1Type.Name
                },

                new EquipmentItem
                {
                    Slot = "2",
                    EfficiencyInit = shipGirl.Eq2EffInit,
                    EfficiencyMax = shipGirl.Eq2EffInitMax,
                    EfficiencyKai = shipGirl.Eq2EffInitKai,
                    Equippable = shipGirl.FK_Eq2Type.Name
                },

                new EquipmentItem
                {
                    Slot = "3",
                    EfficiencyInit = shipGirl.Eq3EffInit,
                    EfficiencyMax = shipGirl.Eq3EffInitMax,
                    EfficiencyKai = shipGirl.Eq3EffInitKai,
                    Equippable = shipGirl.FK_Eq3Type.Name
                }
            };

            SkillsList = shipGirl.Skills;

            if (!string.IsNullOrEmpty(shipGirl.LimitBreak1))
            {
                string listMarker = "\u2022  ";
                LimitBreak1 = listMarker + shipGirl.LimitBreak1.Replace("/ ", "\n" + listMarker);
                LimitBreak2 = listMarker + shipGirl.LimitBreak2.Replace("/ ", "\n" + listMarker);
                LimitBreak3 = listMarker + shipGirl.LimitBreak3.Replace("/ ", "\n" + listMarker);
            }
            
            // DROPS
            ConstructionTime = shipGirl.ConstructTime;

            DropsList = new List<DropItem>
            {
                new DropItem {Map = "1"},
                new DropItem {Map = "2"},
                new DropItem {Map = "3"},
                new DropItem {Map = "4 + SOS"}
            };

            foreach (ShipGirlWhereToGetShipGirl location in shipGirl.WhereToGetShipGirl)
            {
                string locationName = location.FK_WhereToGetShipGirl.Name;

                if (locationName.Contains('-'))
                {
                    string chapter = locationName.Split('-').First();
                    int map = Convert.ToInt32(locationName.Split('-').Last());

                    // Drop
                    DropItem row = DropsList[map - 1];
                    PropertyInfo propertyInfo = row.GetType().GetProperty("Ch_" + chapter);
                    propertyInfo?.SetValue(row, PackIconKind.Check, null);
                }
                else
                {
                    switch (locationName)
                    {
                        case "Light":
                            LightConstructionIcon = PackIconKind.Check;
                            break;
                        case "Heavy":
                            HeavyConstructionIcon = PackIconKind.Check;
                            break;
                        case "Aviation":
                            AviationConstructionIcon = PackIconKind.Check;
                            break;
                        case "Limited":
                            LimitedConstructionIcon = PackIconKind.Check;
                            break;
                        case "Exchange":
                            ExchangeConstructionIcon = PackIconKind.Check;
                            break;
                    }
                }

                ConstructionNote += location.Note;
            }

            if (shipGirl.Remodel.Equals("f"))
            {
                RetrofitVisibility = Visibility.Collapsed;
            }
        }
    }

    /// <summary>
    /// Interaction logic for ShipGirlPage.xaml
    /// </summary>
    public partial class ShipGirlPage : Page
    {
        public ShipGirlPage(ShipGirl shipGirl)
        {
            InitializeComponent();
            DataContext = new ShipGirlPageViewModel(shipGirl);
        }

        private void SkillsListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListView listView = sender as ListView;
            GridView gView = listView.View as GridView;

            var workingWidth = listView.ActualWidth - 20;
            var col1 = 0.07;
            var col2 = 0.20;
            var col3 = 1 - (col2 + col1);

            gView.Columns[0].Width = Math.Abs(workingWidth * col1);
            gView.Columns[1].Width = Math.Abs(workingWidth * col2);
            gView.Columns[2].Width = Math.Abs(workingWidth * col3);
        }

        private void EquipmentList_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListView listView = sender as ListView;
            GridView gView = listView.View as GridView;

            var workingWidth = listView.ActualWidth - 20;
            var col1 = 0.07;
            var col2 = 0.2;
            var col3 = 0.2;

            var col4 = 1 - (col1 + col2 + col3);

            gView.Columns[0].Width = Math.Abs(workingWidth * col1);
            gView.Columns[1].Width = Math.Abs(workingWidth * col2);
            gView.Columns[2].Width = Math.Abs(workingWidth * col3);
            gView.Columns[3].Width = Math.Abs(workingWidth * col4);
        }

        private void StatTable_OnEnter(object sender, MouseEventArgs e)
        {
            var element = (Border)e.Source;
            StatTable.BrushCell(Grid.GetRow(element), Grid.GetColumn(element), Brushes.LightGray);
        }

        private void StatTable_OnLeave(object sender, MouseEventArgs e)
        {
            var element = (Border)e.Source;
            StatTable.BrushCell(Grid.GetRow(element), Grid.GetColumn(element), Brushes.Transparent);
        }
    }
}
