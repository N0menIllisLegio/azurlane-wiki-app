using azurlane_wiki_app.Annotations;
using azurlane_wiki_app.Data.Tables;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace azurlane_wiki_app.PageShipGirlList.Items
{
    public class BaseShipGirlItem : INotifyPropertyChanged
    {
        private string rarity;
        private bool retrofited;
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
        public string RarityIcon { get; set; }
        public string NationalityIcon { get; set; }
        public string Nationality { get; set; }

        public BaseShipGirlItem(ShipGirl shipGirl)
        {
            ShipID = shipGirl.ShipID;
            Name = shipGirl.Name;
            Rarity = shipGirl.FK_Rarity.Name;
            RarityIcon = shipGirl.FK_Rarity.FK_Icon.FileName;
            NationalityIcon = shipGirl.FK_Nationality.FK_Icon.FileName;
            Nationality = shipGirl.FK_Nationality.Name;
            TypeIcon = shipGirl.FK_ShipType.FK_Icon.FileName;
            Type = shipGirl.FK_ShipType.Name;
            Retrofited = false;
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
