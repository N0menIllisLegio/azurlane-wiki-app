using System.ComponentModel;
using azurlane_wiki_app.Data.Tables;

namespace azurlane_wiki_app.PageEquipmentList.Items
{
    public class SubmarineTorpedo : BaseEquipmentItem
    {
        [DisplayName("Torpedo")]
        public int Torp { get; set; }
        public int Rnd { get; set; }
        public int Damage { get; set; }
        public float Reload { get; set; }
        public int Rng { get; set; }
        public string Sprd { get; set; }
        public string Angle { get; set; }
        public string Attr { get; set; }

        //Surface DPS
        //public int Raw { get; set; }
        //public int L { get; set; }
        //public string M { get; set; }
        //public string H { get; set; }

        public SubmarineTorpedo(Equipment equipment) : base(equipment)
        {
            Torp = equipment.TorpMax ?? 0;
            Rnd = equipment.Number ?? 0;
            Damage = equipment.DamageMax ?? 0;
            Reload = equipment.RoFMax ?? 0;
            Rng = equipment.WepRange ?? 0;
            Sprd = $"{equipment.Spread ?? 0}°";
            Angle = $"{equipment.Angle ?? 0}°";
            Attr = equipment.Characteristic;
        }
    }
}
