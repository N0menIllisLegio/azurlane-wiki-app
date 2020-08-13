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

        public TorpedoBomberPlane(Equipment equipment, DPSData dpsData) : base(equipment, dpsData)
        {
            Damage = equipment.DamageMax ?? 0;
            bufDamage = equipment.Damage ?? 0;
            torpedoNumber = equipment.Number ?? 0;

            CalcDPSes(dpsData); // Second time because of Damage
        }

        protected override void CalcBombsDPS(int TorpNumber, int TorpDamage, DPSData dpsData)
        {
            ArmourModifier armourModifier = dpsData.GetTorpedoArmourModifier("Normal");
            double AbsoluteCD = dpsData.Cooldown;

            double raw = TorpNumber * TorpDamage / (Rld * 2.2 + AbsoluteCD);

            SurfacedDPSL = string.Format("{0:0.00}", raw * armourModifier.Light);
            SurfacedDPSM = string.Format("{0:0.00}", raw * armourModifier.Medium);
            SurfacedDPSH = string.Format("{0:0.00}", raw * armourModifier.Heavy);
        }

        public override void ChangeStats(DPSData dpsData)
        {
            Damage = Swap(Damage, ref bufDamage);
            base.ChangeStats(dpsData);
        }

        protected override void CalcDPSes(DPSData dpsData)
        {
            CalcAAGunsDPS(dpsData);
            CalcBombsDPS(torpedoNumber, Damage, dpsData);
        }
    }
}
