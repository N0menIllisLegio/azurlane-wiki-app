using System.ComponentModel;
using azurlane_wiki_app.Data.Tables;

namespace azurlane_wiki_app.PageEquipmentList.Items
{
    public class ASWItem : BaseEquipmentItem
    {
        private string dPS;
        private float rld;
        private int damage;
        private int accuracy;
        private int aSW;

        private float bufRld;
        private int bufDamage;
        private int bufAccuracy;
        private int bufASW;

        [DisplayName("Anti-Submarine Warfare")]
        public int ASW 
        { 
            get => aSW;
            set
            {
                aSW = value;
                OnPropertyChanged(nameof(ASW));
            }
        }
        public int Accuracy 
        { 
            get => accuracy;
            set 
            { 
                accuracy = value;
                OnPropertyChanged(nameof(Accuracy));
            }
        }
        public int Damage 
        { 
            get => damage;
            set
            {
                damage = value;
                OnPropertyChanged(nameof(Damage));
            }
        }

        [DisplayName("Rld(s)")]
        public float Rld 
        { 
            get => rld;
            set
            {
                rld = value;
                OnPropertyChanged(nameof(Rld));
            }
        }
        public string DPS 
        { 
            get => dPS;
            set
            {
                dPS = value;
                OnPropertyChanged(nameof(DPS));
            }
        }
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

            bufASW = equipment.ASW ?? 0;
            bufAccuracy = equipment.Acc ?? 0;
            bufDamage = equipment.Damage ?? 0;
            bufRld = equipment.RoF ?? 0;


            Range = equipment.WepRange ?? 0;
            ASWItemType = equipment.FK_Type.Name;
            Notes = equipment.Notes;

            CalcDMG();
        }

        public override void ChangeStats()
        {
            base.ChangeStats();
            ASW = Swap(ASW, ref bufASW);
            Accuracy = Swap(Accuracy, ref bufAccuracy);
            Damage = Swap(Damage, ref bufDamage);
            Rld = Swap(Rld, ref bufRld);

            CalcDMG();
        }

        private void CalcDMG()
        {
            if (Damage != 0 && Rld != 0)
            {
                DPS = string.Format("{0:0.00}", Damage / Rld);
            }
        }
    }
}
