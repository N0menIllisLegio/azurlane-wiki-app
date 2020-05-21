using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using azurlane_wiki_app.Data;
using azurlane_wiki_app.Data.Tables;
using azurlane_wiki_app.Properties;
using MaterialDesignThemes.Wpf;

namespace azurlane_wiki_app.PageShipGirl
{
    public class ShipGirlPageViewModel : INotifyPropertyChanged
    {
        public string ShipName { get; set; }
        public List<GeneralInfoItem> GeneralInfoList { get; set; }
        public StatTable InitialStats { get; set; }
        public string ShipGirlTypeIcon { get; set; }
        public string ShipGirlSubtypeIcon { get; set; }
        public string ShipGirlType { get; set; }
        public string ShipGirlSubtype { get; set; }

        public string CoinIcon { get; set; }
        public string OilIcon { get; set; }
        public string MedalIcon { get; set; }
        
        public List<EquipmentItem> EquipmentList { get; set; }
        public IEnumerable<Skill> SkillsList { get; set; }

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
        public string LightConstructionNote { get; set; }
        public string HeavyConstructionNote { get; set; } 
        public string AviationConstructionNote { get; set; }
        public string LimitedConstructionNote { get; set; } 
        public string ExchangeConstructionNote { get; set; }

        #endregion

        #region ChangableProperties

        private string _shipGirlIcon;
        private string _chibiImage;
        private string _bannerImage;
        private string _mainImage;
        private string _rarity;
        private string _scrapCoinsValue;
        private string _scrapOilValue;
        private string _scrapMedalsValue;
        private StatTable _level100Stats;
        private StatTable _level120Stats;
        private Visibility _retrofitVisibility = Visibility.Visible;

        public string ShipGirlIcon
        {
            get => _shipGirlIcon;
            set
            {
                _shipGirlIcon = value;
                OnPropertyChanged(nameof(ShipGirlIcon));
            }
        }

        public string ChibiImage
        {
            get => _chibiImage;
            set
            {
                _chibiImage = value;
                OnPropertyChanged(nameof(ChibiImage));
            }
        }

        public string BannerImage
        {
            get => _bannerImage;
            set
            {
                _bannerImage = value;
                OnPropertyChanged(nameof(BannerImage));
            }
        }

        public string MainImage
        {
            get => _mainImage;
            set
            {
                _mainImage = value;
                OnPropertyChanged(nameof(MainImage));
            }
        }

        public string Rarity
        {
            get => _rarity;
            set
            {
                _rarity = value;
                OnPropertyChanged(nameof(Rarity));
            }
        }

        public string ScrapCoinsValue
        {
            get => _scrapCoinsValue;
            set
            {
                _scrapCoinsValue = value;
                OnPropertyChanged(nameof(ScrapCoinsValue));
            }
        }

        public string ScrapOilValue
        {
            get => _scrapOilValue;
            set
            {
                _scrapOilValue = value;
                OnPropertyChanged(nameof(ScrapOilValue));
            }
        }

        public string ScrapMedalsValue
        {
            get => _scrapMedalsValue;
            set
            {
                _scrapMedalsValue = value;
                OnPropertyChanged(nameof(ScrapMedalsValue));
            }
        }

        public StatTable Level100Stats
        {
            get => _level100Stats;
            set
            {
                _level100Stats = value;
                OnPropertyChanged(nameof(Level100Stats));
            }
        }

        public StatTable Level120Stats
        {
            get => _level120Stats;
            set
            {
                _level120Stats = value;
                OnPropertyChanged(nameof(Level120Stats));
            }
        }

        public Visibility RetrofitVisibility
        {
            get => _retrofitVisibility;
            set
            {
                _retrofitVisibility = value;
                OnPropertyChanged(nameof(RetrofitVisibility));
            }
        }

        #endregion

        public RelayCommand RetrofitCommand { get; set; }
        public RelayCommand UnretrofitCommand { get; set; }
        public RelayCommand ClosePageCommand { get; set; }

        public ShipGirlPageViewModel(string shipGirlId)
        {
            ClosePageCommand = new RelayCommand(obj =>
            {
                Navigation.Service.GoBack();
            });

            CargoContext cargoContext = new CargoContext();

            ShipGirl shipGirl = cargoContext.ShipGirls.Find(shipGirlId);
            
            if (shipGirl == null)
            {
                return;
            }

            // GENERAL INFO
            ShipName = shipGirl.Name;

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
                    Description = null,
                    Icon = null,
                    IconHeight = 25,
                    IconWidth = 50
                }
            };

            DisplayStandardParameters(shipGirl);

            ShipGirlTypeIcon = shipGirl.FK_ShipType.FK_Icon.FileName;
            ShipGirlType = shipGirl.FK_ShipType.Name;

            if (shipGirl.FK_SubtypeRetro != null)
            {
                ShipGirlSubtypeIcon = shipGirl.FK_SubtypeRetro.FK_Icon.FileName;
                ShipGirlSubtype = shipGirl.FK_SubtypeRetro.Name;
            }

            // EQUIPMENT
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

            // SKILLS
            SkillsList = shipGirl.Skills;

            // TODO: add animation of image transition

            // LIMIT BREAKS
            if (!string.IsNullOrEmpty(shipGirl.LimitBreak1))
            {
                string listMarker = "\u2022  ";
                LimitBreak1 = listMarker + shipGirl.LimitBreak1.Replace("/ ", "\n" + listMarker);
                LimitBreak2 = listMarker + shipGirl.LimitBreak2.Replace("/ ", "\n" + listMarker);
                LimitBreak3 = listMarker + shipGirl.LimitBreak3.Replace("/ ", "\n" + listMarker);
            }
            
            // DROPS
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
                string locationNote = string.IsNullOrEmpty(location.Note) ? null : location.Note;
                
                PackIconKind icon = string.IsNullOrEmpty(locationNote)
                    ? PackIconKind.Check
                    : PackIconKind.CalendarText;

                if (locationName.Contains('-'))
                {
                    string chapter = locationName.Split('-').First();
                    int map = Convert.ToInt32(locationName.Split('-').Last());

                    DropItem row = DropsList[map - 1];
                    PropertyInfo propertyInfo = row.GetType().GetProperty("Ch_" + chapter);
                    propertyInfo?.SetValue(row, icon, null);

                    propertyInfo = row.GetType().GetProperty("ToolTip_" + chapter);
                    propertyInfo?.SetValue(row, locationNote, null);
                }
                else
                {
                    switch (locationName)
                    {
                        case "Light":
                            LightConstructionIcon = icon;
                            LightConstructionNote = locationNote;
                            break;
                        case "Heavy":
                            HeavyConstructionIcon = icon;
                            HeavyConstructionNote = locationNote;
                            break;
                        case "Aviation":
                            AviationConstructionIcon = icon;
                            AviationConstructionNote = locationNote;
                            break;
                        case "Limited":
                            LimitedConstructionIcon = icon;
                            LimitedConstructionNote = locationNote;
                            break;
                        case "Exchange":
                            ExchangeConstructionIcon = icon;
                            ExchangeConstructionNote = locationNote;
                            break;
                    }
                }
            }

            // RETROFIT
            if (shipGirl.Remodel.Equals("f"))
            {
                RetrofitVisibility = Visibility.Collapsed;
            }
            else
            {
                RetrofitVisibility = Visibility.Visible;

                RetrofitCommand = new RelayCommand(obj =>
                {
                    DisplayRetrofitParameters(shipGirl);
                });

                UnretrofitCommand = new RelayCommand(obj =>
                {
                    DisplayStandardParameters(shipGirl);
                });
            }

            InitialStats = new StatTable("Base Stats", shipGirl.HealthInitial.ToString(),
                shipGirl.Armor, shipGirl.ReloadInitial.ToString(), shipGirl.Luck.ToString(),
                shipGirl.FireInitial.ToString(), shipGirl.TorpInitial.ToString(), shipGirl.EvadeInitial.ToString(),
                shipGirl.Speed.ToString(), shipGirl.AirInitial.ToString(), shipGirl.ConsumptionInitial.ToString(),
                shipGirl.AAInitial.ToString(), shipGirl.AccInitial.ToString(), shipGirl.ASWInitial.ToString(),
                shipGirl.OxygenInitial.ToString(), shipGirl.AmmoInitial.ToString());
        }

        private void DisplayStandardParameters(ShipGirl shipGirl)
        {
            ShipGirlIcon = shipGirl.ImageIcon;
            MainImage = shipGirl.Image;

            ChibiImage = shipGirl.ImageChibi;
            BannerImage = shipGirl.ImageBanner;

            Rarity = shipGirl.FK_Rarity.Name;

            GeneralInfoItem rarityItem = GeneralInfoList.LastOrDefault();

            if (rarityItem != null)
            {
                rarityItem.Description = Rarity;
                rarityItem.Icon = shipGirl.FK_Rarity.FK_Icon.FileName;
            }
            
            Level100Stats = new StatTable("Level 100", shipGirl.HealthMax.ToString(),
                shipGirl.Armor, shipGirl.ReloadMax.ToString(), shipGirl.Luck.ToString(),
                shipGirl.FireMax.ToString(), shipGirl.TorpMax.ToString(), shipGirl.EvadeMax.ToString(),
                shipGirl.Speed.ToString(), shipGirl.AirMax.ToString(), shipGirl.ConsumptionMax.ToString(),
                shipGirl.AAMax.ToString(), shipGirl.AccMax.ToString(), shipGirl.ASWMax.ToString(),
                shipGirl.OxygenMax.ToString(), shipGirl.AmmoMax.ToString());

            Level120Stats = new StatTable("Level 120", shipGirl.Health120.ToString(),
                shipGirl.Armor, shipGirl.Reload120.ToString(), shipGirl.Luck.ToString(),
                shipGirl.Fire120.ToString(), shipGirl.Torp120.ToString(), shipGirl.Evade120.ToString(),
                shipGirl.Speed.ToString(), shipGirl.Air120.ToString(), shipGirl.Consumption120.ToString(),
                shipGirl.AA120.ToString(), shipGirl.Acc120.ToString(), shipGirl.ASW120.ToString(),
                shipGirl.Oxygen120.ToString(), shipGirl.Ammo120.ToString());

            if (Rarity != "Priority" && Rarity != "Decisive" && Rarity != "Unreleased")
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
        }

        private void DisplayRetrofitParameters(ShipGirl shipGirl)
        {
            ShipGirlIcon = shipGirl.ImageIconKai;
            MainImage = shipGirl.ImageKai;

            ChibiImage = shipGirl.ImageChibiKai;
            BannerImage = shipGirl.ImageBannerKai;

            RarityConverter rarityConverter = new RarityConverter();
            Rarity = rarityConverter.Convert(shipGirl.FK_Rarity.Name, null, "1", null)?.ToString();

            GeneralInfoItem rarityItem = GeneralInfoList.LastOrDefault();
            if (rarityItem != null)
            {
                rarityItem.Description = Rarity;
                using (CargoContext cargoContext = new CargoContext())
                {
                    rarityItem.Icon = cargoContext.Icons.Find(Rarity)?.FileName;
                }
            }

            Level120Stats = new StatTable("Level 120", shipGirl.HealthKai120.ToString(),
                shipGirl.ArmorKai, shipGirl.ReloadKai120.ToString(), shipGirl.Luck.ToString(),
                shipGirl.FireKai120.ToString(), shipGirl.TorpKai120.ToString(), shipGirl.EvadeKai120.ToString(),
                shipGirl.SpeedKai.ToString(), shipGirl.AirKai120.ToString(), shipGirl.ConsumptionKai120.ToString(),
                shipGirl.AAKai120.ToString(), shipGirl.AccKai120.ToString(), shipGirl.ASWKai120.ToString(),
                shipGirl.OxygenKai120.ToString(), shipGirl.AmmoKai120.ToString());

            Level100Stats = new StatTable("Level 100", shipGirl.HealthKai.ToString(),
                shipGirl.ArmorKai, shipGirl.ReloadKai.ToString(), shipGirl.Luck.ToString(),
                shipGirl.FireKai.ToString(), shipGirl.TorpKai.ToString(), shipGirl.EvadeKai.ToString(),
                shipGirl.SpeedKai.ToString(), shipGirl.AirKai.ToString(), shipGirl.ConsumptionKai.ToString(),
                shipGirl.AAKai.ToString(), shipGirl.AccKai.ToString(), shipGirl.ASWKai.ToString(),
                shipGirl.OxygenKai.ToString(), shipGirl.AmmoKai.ToString());

            if (Rarity != "Priority" && Rarity != "Decisive" && Rarity != "Unreleased")
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
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}