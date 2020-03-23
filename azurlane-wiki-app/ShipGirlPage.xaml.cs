using azurlane_wiki_app.Data.Tables;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace azurlane_wiki_app
{
    public struct GeneralInfoItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public BitmapImage Icon { get; set; }
        public int IconWidth { get; set; }
        public int IconHeight { get; set; }
    }

    public struct EquipmentItem
    {
        public string Slot { get; set; }
        public string Efficiency { get; set; }
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
            AaStatValue = aaStatValue;
            AccStatValue = accStatValue;
            ASWStatValue = aswStatValue;
            OxygenStatValue = oxygenStatValue;
            AmmoStatValue = ammoStatValue;

            using (CargoContext cargoContext = new CargoContext())
            {
                HealthStatIcon = getBitmapImage(cargoContext.Icons.Find("Health")?.FileName);
                ArmorStatIcon = getBitmapImage(cargoContext.Icons.Find("Armor")?.FileName);
                ReloadStatIcon = getBitmapImage(cargoContext.Icons.Find("Reload")?.FileName);
                LuckStatIcon = getBitmapImage(cargoContext.Icons.Find("Luck")?.FileName);
                FireStatIcon = getBitmapImage(cargoContext.Icons.Find("Firepower")?.FileName);
                TorpStatIcon = getBitmapImage(cargoContext.Icons.Find("Torpedo")?.FileName);
                EvadeStatIcon = getBitmapImage(cargoContext.Icons.Find("Evasion")?.FileName);
                AAStatIcon = getBitmapImage(cargoContext.Icons.Find("Anti-air")?.FileName);
                AirStatIcon = getBitmapImage(cargoContext.Icons.Find("Aviation")?.FileName);
                ConsumptionStatIcon = getBitmapImage(cargoContext.Icons.Find("Oil consumption")?.FileName);
                AccStatIcon = getBitmapImage(cargoContext.Icons.Find("Accuracy")?.FileName);
                ASWStatIcon = getBitmapImage(cargoContext.Icons.Find("Anti-submarine warfare")?.FileName);
                OxygenStatIcon = getBitmapImage(cargoContext.Icons.Find("Oxygen")?.FileName);
                AmmoStatIcon = getBitmapImage(cargoContext.Icons.Find("Ammunition")?.FileName);
                HuntingRangeStatIcon = getBitmapImage(cargoContext.Icons.Find("Hunting range")?.FileName);
            }


            Width = Height = 25;
        }

        public string StatTableTitle { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        // ICONS
        public BitmapImage HealthStatIcon { get; set; }
        public BitmapImage ArmorStatIcon { get; set; }
        public BitmapImage ReloadStatIcon { get; set; }
        public BitmapImage LuckStatIcon { get; set; }

        public BitmapImage FireStatIcon { get; set; }
        public BitmapImage TorpStatIcon { get; set; }
        public BitmapImage EvadeStatIcon { get; set; }

        public BitmapImage AAStatIcon { get; set; }
        public BitmapImage AirStatIcon { get; set; }
        public BitmapImage ConsumptionStatIcon { get; set; }
        public BitmapImage AccStatIcon { get; set; }

        public BitmapImage ASWStatIcon { get; set; }
        public BitmapImage OxygenStatIcon { get; set; }
        public BitmapImage AmmoStatIcon { get; set; }
        public BitmapImage HuntingRangeStatIcon { get; set; }

        // STATS
        public string HealthStatValue { get; set; }
        public string ArmorStatValue { get; set; }
        public string ReloadStatValue { get; set; }
        public string LuckStatValue { get; set; }

        public string FireStatValue { get; set; }
        public string TorpStatValue { get; set; }
        public string EvadeStatValue { get; set; }
        public string SpeedStatValue { get; set; }

        public string AaStatValue { get; set; }
        public string AirStatValue { get; set; }
        public string ConsumptionStatValue { get; set; }
        public string AccStatValue { get; set; }

        public string ASWStatValue { get; set; }
        public string OxygenStatValue { get; set; }
        public string AmmoStatValue { get; set; }

        private static BitmapImage getBitmapImage(string path)
        {
            return new BitmapImage(new Uri(@"pack://siteoforigin:,,,"
                                           + path.Remove(0, 1)));
        }
    }

    public class ShipGirlPageViewModel
    {
        public string ShipName { get; set; }
        public BitmapImage ShipGirlIcon { get; set; }

        public BitmapImage ChibiImage { get; set; }
        public BitmapImage BannerImage { get; set; }
        public BitmapImage MainImage { get; set; }
        public List<GeneralInfoItem> GeneralInfoList { get; set; }
        public BitmapImage ShipGirlTypeIcon { get; set; }
        public BitmapImage ShipGirlSubtypeIcon { get; set; }
        public Visibility ClassificationChevron { get; set; }
        public string ShipGirlType { get; set; }
        public string ShipGirlSubtype { get; set; }
        public BitmapImage CoinIcon { get; set; }
        public BitmapImage OilIcon { get; set; }
        public BitmapImage MedalIcon { get; set; }
        public string ScrapCoinsValue { get; set; }
        public string ScrapOilValue { get; set; }
        public string ScrapMedalsValue { get; set; }

        public StatTable InitialStats { get; set; }
        public StatTable Level100Stats { get; set; }
        public StatTable Level120Stats { get; set; }

        public List<EquipmentItem> EquipmentList { get; set; }

        public IEnumerable<Skill> SkillsList { get; set; }

        public string Rarity { get; set; }

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
            ShipGirlIcon = getBitmapImage(shipGirl.ImageIcon);
            MainImage = getBitmapImage(shipGirl.Image);

            ChibiImage = getBitmapImage(shipGirl.ImageChibi);
            BannerImage = getBitmapImage(shipGirl.ImageBanner);

            GeneralInfoList = new List<GeneralInfoItem>
            {
                new GeneralInfoItem {Name = "ID", Description = shipGirl.ShipID},
                new GeneralInfoItem {Name = "Class", Description = shipGirl.FK_ShipClass.Name},
                new GeneralInfoItem {Name = "Construction Time", Description = shipGirl.ConstructTime},
                new GeneralInfoItem
                {
                    Name = "Nationality", 
                    Description = shipGirl.FK_Nationality.Name,
                    Icon = getBitmapImage(shipGirl.FK_Nationality.FK_Icon.FileName),
                    IconHeight = 52,
                    IconWidth = 50
                },
                new GeneralInfoItem
                {
                    Name = "Rarity", 
                    Description = shipGirl.FK_Rarity.Name,
                    Icon = getBitmapImage(shipGirl.FK_Rarity.FK_Icon.FileName),
                    IconHeight = 25,
                    IconWidth = 50
                }
            };

            Rarity = shipGirl.FK_Rarity.Name;
            ShipGirlTypeIcon = getBitmapImage(shipGirl.FK_ShipType.FK_Icon.FileName);
            ShipGirlType = shipGirl.FK_ShipType.Name;

            if (shipGirl.FK_SubtypeRetro != null)
            {
                ShipGirlSubtypeIcon = getBitmapImage(shipGirl.FK_SubtypeRetro.FK_Icon.FileName);
                ShipGirlSubtype = shipGirl.FK_SubtypeRetro.Name;
                ClassificationChevron = Visibility.Visible;
            }
            else
            {
                ClassificationChevron = Visibility.Collapsed;
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
                    CoinIcon = getBitmapImage(cargoContext.Icons.Find("Coin")?.FileName);
                    OilIcon = getBitmapImage(cargoContext.Icons.Find("Oil")?.FileName);
                    MedalIcon = getBitmapImage(cargoContext.Icons.Find("Medal")?.FileName);
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
                    Efficiency = shipGirl.Eq1EffInit + "->" + shipGirl.Eq1EffInitMax + "->" + shipGirl.Eq1EffInitKai,
                    Equippable = shipGirl.Eq1Type
                },
                new EquipmentItem
                {
                    Slot = "2",
                    Efficiency = shipGirl.Eq2EffInit + "->" + shipGirl.Eq2EffInitMax + "->" + shipGirl.Eq2EffInitKai,
                    Equippable = shipGirl.Eq2Type
                },
                new EquipmentItem
                {
                    Slot = "3",
                    Efficiency = shipGirl.Eq3EffInit + "->" + shipGirl.Eq3EffInitMax + "->" + shipGirl.Eq3EffInitKai,
                    Equippable = shipGirl.Eq3Type
                }
            };

            SkillsList = shipGirl.Skills;

            LimitBreak1 = shipGirl.LimitBreak1;
            LimitBreak2 = shipGirl.LimitBreak2;
            LimitBreak3 = shipGirl.LimitBreak3;

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
        }

        private BitmapImage getBitmapImage(string path)
        {
            return new BitmapImage(new Uri(@"pack://siteoforigin:,,,"
                                           + path.Remove(0, 1)));
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
            var col1 = 0.20;
            var col2 = 0.80;

            gView.Columns[0].Width = Math.Abs(workingWidth * col1);
            gView.Columns[1].Width = Math.Abs(workingWidth * col2);
        }
    }
}
