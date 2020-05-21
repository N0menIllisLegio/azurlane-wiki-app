using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using azurlane_wiki_app.Data;
using azurlane_wiki_app.Data.Tables;
using azurlane_wiki_app.Properties;
using MaterialDesignThemes.Wpf;

namespace azurlane_wiki_app.PageEquipment
{
    public class EquipmentPageViewModel : INotifyPropertyChanged
    {
        Dictionary<int, string> StarsRarityDictionary = new Dictionary<int, string>
        {
            [1] = "Normal",
            [2] = "Normal",
            [3] = "Rare",
            [4] = "Elite",
            [5] = "Super Rare",
            [6] = "Ultra Rare"
        };

        public string Image { get; set; }
        public string EquipmentName { get; set; }
        public string EquipmentType { get; set; }
        public string EquipmentTypeIcon { get; set; }
        public string Nation { get; set; }
        public string NationIcon { get; set; }
        public string Notes { get; set; }
        public string DropLocation { get; set; }

        public string Rarity { get; set; }
        public string RarityIcon { get; set; }

        public List<EquipmentUsedBy> UsedByList { get; set; }
        public List<EquipmentStats> StatsList { get; set; }

        public RelayCommand ClosePageCommand { get; set; }


        public EquipmentPageViewModel(int id)
        {
            ClosePageCommand = new RelayCommand(obj =>
            {
                Navigation.Service.GoBack();
            });

            using (CargoContext cargoContext = new CargoContext())
            {
                Equipment equipment = cargoContext.ShipGirlsEquipment.Find(id);

                EquipmentName = equipment.Name;
                Rarity = StarsRarityDictionary[(int) equipment.Stars];
                Image = equipment.Image;
                EquipmentType = equipment.Type;

                DropLocation = string.IsNullOrEmpty(equipment.DropLocation)
                    ? null
                    : equipment.DropLocation;

                Notes = string.IsNullOrEmpty(equipment.Notes)
                    ? null
                    : equipment.Notes;

                Nation = equipment.FK_Nationality.Name;
                NationIcon = equipment.FK_Nationality.FK_Icon.FileName;
                UsedByList = new List<EquipmentUsedBy>();


                RarityIcon = cargoContext.Icons.Find(Rarity)?.FileName;

                foreach (ShipType shipType in cargoContext.ShipTypes)
                {
                    string kind = (string) equipment.GetType().GetProperty(shipType.Abbreviation)?.GetValue(equipment);
                    string note = (string) equipment.GetType().GetProperty(shipType.Abbreviation + "Note")
                        ?.GetValue(equipment);

                    EquipmentUsedBy listItem = new EquipmentUsedBy();

                    switch (kind)
                    {
                        case "2":
                            listItem.Kind = "CalendarText";
                            break;
                        case "1":
                            listItem.Kind = "Check";
                            break;
                        default:
                            listItem.Kind = "Close";
                            break;
                    }

                    listItem.Note = string.IsNullOrEmpty(note) ? null : note;
                    listItem.Icon = shipType.FK_Icon.FileName;
                    listItem.Name = shipType.Name;

                    UsedByList.Add(listItem);
                }

                FillStatsList(equipment);
            }
        }

        private void FillStatsList(Equipment equipment)
        {
            StatsList = new List<EquipmentStats>();

            CreateRowIfPossible("Health", equipment.Health?.ToString(), equipment.HealthMax?.ToString());
            CreateRowIfPossible("Torpedo", equipment.Torpedo?.ToString(), equipment.TorpMax?.ToString());
            CreateRowIfPossible("Firepower", equipment.Firepower?.ToString(), equipment.FPMax?.ToString());
            CreateRowIfPossible("Aviation", equipment.Aviation?.ToString(), equipment.AvMax?.ToString());
            CreateRowIfPossible("Evasion", equipment.Evasion?.ToString(), equipment.EvasionMax?.ToString());
            CreateRowIfPossible("Anti-submarine warfare", equipment.ASW?.ToString(), equipment.ASWMax?.ToString());
            CreateRowIfPossible("Oxygen", equipment.Oxygen?.ToString(), equipment.OxygenMax?.ToString());
            CreateRowIfPossible("Anti-air", equipment.AA?.ToString(), equipment.AAMax?.ToString());
            CreateRowIfPossible("Reload", equipment.Reload?.ToString(), equipment.ReloadMax?.ToString());
            CreateRowIfPossible("Luck", equipment.Luck?.ToString(), equipment.LuckMax?.ToString());
            CreateRowIfPossible("Accuracy", equipment.Acc?.ToString(), equipment.AccMax?.ToString());
            CreateRowIfPossible("Speed", equipment.Spd?.ToString(), equipment.SpdMax?.ToString());
            CreateRowIfPossible("Plane Health", equipment.PlaneHP?.ToString(), equipment.PlaneHPMax?.ToString());
            CreateRowIfPossible("Damage", equipment.Damage?.ToString(), equipment.DamageMax?.ToString());

            CreateRowIfPossible("Rate of Fire",
                FormatDescription("{0}s", equipment.RoF?.ToString()),
                FormatDescription("{0}s", equipment.RoFMax?.ToString()));

            CreateRowIfPossible("No. of Torpedoes",
                FormatDescription("{0} torpedoes", equipment.Number?.ToString()));

            CreateRowIfPossible("Spread",
                FormatDescription("{0}°", equipment.Spread?.ToString()));

            CreateRowIfPossible("Angle",
                FormatDescription("{0}°", equipment.Angle?.ToString()));

            CreateRowIfPossible("Range", equipment.WepRange?.ToString());

            CreateRowIfPossible("Volley",
                FormatDescription("{0} x {1} Shells",
                    equipment.Salvoes?.ToString(), equipment.Shells?.ToString()));

            CreateRowIfPossible("Volley Time",
                FormatDescription("{0}s", equipment.VolleyTime?.ToString()));

            CreateRowIfPossible("Coefficient",
                FormatDescription("{0}%", equipment.Coef?.ToString()));

            CreateRowIfPossible("Ammo Type", equipment.Ammo);

            CreateRowIfPossible("Ping Frequency",
                FormatDescription("{0}s per sweep", equipment.PingFreq?.ToString()));

            CreateRowIfPossible("Characteristic", equipment.Characteristic);

            CreateRowIfPossible("AA Guns",
                FormatDescription("{0}\n{1}", equipment.AAGun1, equipment.AAGun2));

            CreateRowIfPossible("Ordnance",
                FormatDescription("{0}\n{1}", equipment.Bombs1, equipment.Bombs2));
        }

        private string FormatDescription(string format, params string[] descriptions)
        {
            if (descriptions.Length < 1)
            {
                return null;
            }

            if (descriptions.Count(string.IsNullOrEmpty) == descriptions.Length)
            {
                return null;
            }

            return string.Format(format, descriptions);
        }

        private void CreateRowIfPossible(string name, string firstDescription, string secondDescription = null,
            string icon = null)
        {
            if (!string.IsNullOrEmpty(firstDescription))
            {
                if (icon == null)
                {
                    icon = name;
                }

                using (CargoContext cargoContext = new CargoContext())
                {
                    StatsList.Add(new EquipmentStats
                    {
                        Name = name,
                        Icon = cargoContext.Icons.Find(icon)?.FileName,
                        DescriptionFirstPart = firstDescription,
                        DescriptionSecondPart = secondDescription
                    });
                }
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