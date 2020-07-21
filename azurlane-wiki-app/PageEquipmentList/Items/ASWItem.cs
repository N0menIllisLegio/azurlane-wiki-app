using System.ComponentModel;
using azurlane_wiki_app.Data.Tables;

namespace azurlane_wiki_app.PageEquipmentList.Items
{
    public class ASWItem : BaseEquipmentItem
    {
        [DisplayName("Anti-Submarine Warfare")]
        public int ASW { get; set; }
        public int Accuracy { get; set; }
        public int Damage { get; set; }

        [DisplayName("Rld(s)")]
        public float Rld { get; set; }
        public string DPS { get; set; }
        public int Range { get; set; }

        //[DisplayName("AoE Radius")]
        //public int AoE { get; set; }

        [DisplayName("Type")]
        public string ASWItemType { get; set; }
        public string Notes { get; set; }

        
        public ASWItem(Equipment equipment) : base(equipment)
        {
            ASW = equipment.ASWMax ?? 0;
            Accuracy = equipment.AccMax ?? 0;
            Damage = equipment.DamageMax ?? 0;
            Rld = equipment.RoFMax ?? 0;
            Range = equipment.WepRange ?? 0;
            ASWItemType = equipment.FK_Type.Name;
            Notes = equipment.Notes;

            if (Damage != 0 && Rld != 0)
            {
                DPS = string.Format("{0:0.00}", Damage / Rld);
            }
        }
    }
}
