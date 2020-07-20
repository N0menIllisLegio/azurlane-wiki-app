using azurlane_wiki_app.Data.Tables;
using System.ComponentModel;

namespace azurlane_wiki_app.PageEquipmentList.Items
{
    public class AAGun : BaseEquipmentItem
    {
        public int Firepower { get; set; }

        [DisplayName("Anti-Air")]
        public int AA { get; set; }
        public int Accuracy { get; set; }
        public int Damage { get; set; }
        public float Reload { get; set; }
        public int Rng { get; set; }

        public string AADPS { get; set; }

        public AAGun(Equipment equipment) : base(equipment)
        {
            Firepower = equipment.Firepower ?? 0;
            AA = equipment.AA ?? 0;
            Accuracy = equipment.Acc ?? 0;
            Damage = equipment.DamageMax ?? 0;
            Reload = equipment.RoFMax ?? 0;
            Rng = equipment.WepRange ?? 0;

            double AbsoluteCD = .5;

            AADPS = string.Format("{0:0.00}", Damage / (Reload + AbsoluteCD));
        }
    }
}
