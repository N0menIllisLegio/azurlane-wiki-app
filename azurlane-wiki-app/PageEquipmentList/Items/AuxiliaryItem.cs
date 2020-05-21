using azurlane_wiki_app.Data.Tables;
using System.ComponentModel;

namespace azurlane_wiki_app.PageEquipmentList.Items
{
    public class AuxiliaryItem : BaseEquipmentItem
    { 
        public int Health { get; set; }
        public int Firepower { get; set; }

        [DisplayName("Anti-Air")]
        public int AA { get; set; }

        [DisplayName("Torpedo")]
        public int Torp { get; set; }
        public int Aviation { get; set; }
        public float Reload { get; set; }
        public int Evasion { get; set; }
        public int Oxygen { get; set; }
        public int Accuracy { get; set; }
        public int Speed { get; set; }
        public string Note { get; set; }

        public AuxiliaryItem(Equipment equipment) : base(equipment)
        {
            Health = equipment.HealthMax ?? 0;
            Firepower = equipment.FPMax ?? 0;
            AA = equipment.AAMax ?? 0;
            Torp = equipment.TorpMax ?? 0;
            Torp = equipment.AvMax ?? 0;
            Reload = equipment.RoFMax ?? 0;
            Evasion = equipment.EvasionMax ?? 0;
            Oxygen = equipment.OxygenMax ?? 0;
            Accuracy = equipment.AccMax ?? 0;
            Speed = equipment.SpdMax ?? 0;
            Note = equipment.Notes;
        }
    }
}
