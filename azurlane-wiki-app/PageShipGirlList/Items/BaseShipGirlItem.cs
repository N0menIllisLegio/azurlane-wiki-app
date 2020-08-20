using azurlane_wiki_app.Properties;
using azurlane_wiki_app.Data.Tables;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace azurlane_wiki_app.PageShipGirlList.Items
{
    public class BaseShipGirlItem : INotifyPropertyChanged
    {
        private string rarity;
        private bool retrofited;
        private string rarityIcon;

        private string bufRarityIcon;
        private string bufRarity;

        public bool Retrofited
        {
            get => retrofited;
            set
            {
                retrofited = value;
                OnPropertyChanged(nameof(Retrofited));
            }

        }
        public string Name { get; set; }
        public string ShipID { get; set; }
        public string TypeIcon { get; set; }
        public string Type { get; set; }
        public string Rarity
        {
            get => rarity;
            set
            {
                rarity = value;
                OnPropertyChanged(nameof(Rarity));
            }
        }
        public string RarityIcon 
        { 
            get => rarityIcon;
            set
            {
                rarityIcon = value;
                OnPropertyChanged(nameof(RarityIcon));
            }
        }
        public string NationalityIcon { get; set; }
        public string Nationality { get; set; }
        public bool Remodel { get; set; }

        public BaseShipGirlItem(ShipGirl shipGirl, string name = null, string iconPath = null)
        {
            ShipID = shipGirl.ShipID;
            Name = shipGirl.Name;
            Rarity = shipGirl.FK_Rarity.Name;
            RarityIcon = shipGirl.FK_Rarity.FK_Icon.FileName;
            NationalityIcon = shipGirl.FK_Nationality.FK_Icon.FileName;
            Nationality = shipGirl.FK_Nationality.Name;
            TypeIcon = shipGirl.FK_ShipType.FK_Icon.FileName;
            Type = shipGirl.FK_ShipType.Name;
            Remodel = shipGirl.Remodel == "t";

            if (Remodel)
            {
                bufRarity = name;
                bufRarityIcon = iconPath;
            }

            Retrofited = false;
        }

        public virtual void InvertRetrofit()
        {
            if (Remodel)
            {
                Retrofited = !Retrofited;

                Rarity = Swap(Rarity, ref bufRarity);
                RarityIcon = Swap(RarityIcon, ref bufRarityIcon);
            }
        }

        protected T Swap<T>(T prop, ref T bufField)
        {
            var temp = bufField;
            bufField = prop;
            return temp;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
