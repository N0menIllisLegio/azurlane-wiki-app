using azurlane_wiki_app.Data.Tables;
using System.ComponentModel;

namespace azurlane_wiki_app.PageEquipmentList.Items
{
    public class AAGun : BaseEquipmentItem
    {
        private int damage;
        private float reload;
        private int bufDamage;
        private float bufReload;
        private string aADPS;

        public int Firepower { get; set; }

        [DisplayName("Anti-Air")]
        public int AA { get; set; }
        public int Accuracy { get; set; }
        public int Damage
        {
            get => damage;
            set
            {
                damage = value;
                OnPropertyChanged(nameof(Damage));
            }
        }
        public float Reload
        {
            get => reload;
            set
            {
                reload = value;
                OnPropertyChanged(nameof(Reload));
            }
        }
        [DisplayName("Anti-Air\nDPS")]
        public string AADPS 
        { 
            get => aADPS;
            set
            {
                aADPS = value;
                OnPropertyChanged(nameof(AADPS));
            }
        }
        public int Rng { get; set; }

        public AAGun(Equipment equipment) : base(equipment)
        {
            Firepower = equipment.Firepower ?? 0;
            AA = equipment.AA ?? 0;
            Accuracy = equipment.Acc ?? 0;

            Damage = equipment.DamageMax ?? 0;
            Reload = equipment.RoFMax ?? 0;

            bufDamage = equipment.Damage ?? 0;
            bufReload = equipment.RoF ?? 0;

            Rng = equipment.WepRange ?? 0;

            CalcDPS();
        }

        public override void ChangeStats()
        {
            base.ChangeStats();
            Damage = Swap(Damage, ref bufDamage);
            Reload = Swap(Reload, ref bufReload);

            CalcDPS();
        }

        private void CalcDPS()
        {
            double AbsoluteCD = .5;

            AADPS = string.Format("{0:0.00}", Damage / (Reload + AbsoluteCD));
        }
    }
}
