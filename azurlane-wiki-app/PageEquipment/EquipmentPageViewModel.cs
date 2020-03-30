using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using azurlane_wiki_app.Data;
using azurlane_wiki_app.Data.Tables;
using azurlane_wiki_app.Properties;

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

        public string EquipmentName { get; set; }
        public string EquipmentType { get; set; }
        public string EquipmentTypeIcon { get; set; }
        public string Nation { get; set; }
        public string NationIcon { get; set; }

        public string Rarity { get; set; }
        public string RarityIcon { get; set; }


        public List<EquipmentUsedBy> UsedByList { get; set; }

        public string Image { get; set; }
        public EquipmentPageViewModel(Equipment equipment)
        {
            EquipmentName = equipment.Name;
            Rarity = StarsRarityDictionary[(int) equipment.Stars];
            Image = equipment.Image;
            EquipmentType = equipment.Type;

            Nation = equipment.FK_Nationality.Name;
            NationIcon = equipment.FK_Nationality.FK_Icon.FileName;

            using (CargoContext cargoContext = new CargoContext())
            {
                RarityIcon = cargoContext.Icons.Find(Rarity)?.FileName;
            }

            // UsedByList.Add();


        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
