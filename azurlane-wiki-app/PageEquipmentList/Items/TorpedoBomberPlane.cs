using azurlane_wiki_app.Data.Tables;

namespace azurlane_wiki_app.PageEquipmentList.Items
{
    public class TorpedoBomberPlane : Plane
    {
        private int damage;
        private int bufDamage;
        private int torpedoNumber;

        public int Damage 
        { 
            get => damage;
            set
            {
                damage = value;
                OnPropertyChanged(nameof(Damage));
            }
        }

        public TorpedoBomberPlane(Equipment equipment) : base(equipment)
        {
            Damage = equipment.DamageMax ?? 0;
            bufDamage = equipment.Damage ?? 0;
            torpedoNumber = equipment.Number ?? 0;

            CalcDPSes(); // Second time because of Damage
        }

        protected override void CalcBombsDPS(int TorpNumber, int TorpDamage)
        {
            double ArmourModifierLight = .8;
            double ArmourModifierMedium = 1.1;
            double ArmourModifierHeavy = 1.3;
            double AbsoluteCD = .1;

            double raw = TorpNumber * TorpDamage / (Rld * 2.2 + AbsoluteCD);

            SurfacedDPSL = string.Format("{0:0.00}", raw * ArmourModifierLight);
            SurfacedDPSM = string.Format("{0:0.00}", raw * ArmourModifierMedium);
            SurfacedDPSH = string.Format("{0:0.00}", raw * ArmourModifierHeavy);
        }

        public override void ChangeStats()
        {
            Damage = Swap(Damage, ref bufDamage);
            base.ChangeStats();
        }

        protected override void CalcDPSes()
        {
            CalcAAGunsDPS();
            CalcBombsDPS(torpedoNumber, Damage);
        }
    }
}
