using azurlane_wiki_app.Data.Tables;

namespace azurlane_wiki_app.PageEquipmentList.Items
{
    public class TorpedoBomberPlane : Plane
    {
        public int Damage { get; set; }

        public TorpedoBomberPlane(Equipment equipment) : base(equipment)
        {
            Damage = equipment.DamageMax ?? 0;
        }

        protected override void CalcBombsDPS(Equipment equipment)
        {
            double ArmourModifierLight = .8;
            double ArmourModifierMedium = 1.1;
            double ArmourModifierHeavy = 1.3;
            double AbsoluteCD = .1;

            int TorpedoNumbers = equipment.Number ?? 0;
            int TorpedoDamage = equipment.DamageMax ?? 0;

            double raw = TorpedoNumbers * TorpedoDamage / (Rld * 2.2 + AbsoluteCD);

            SurfacedDPSL = string.Format("{0:0.00}", raw * ArmourModifierLight);
            SurfacedDPSM = string.Format("{0:0.00}", raw * ArmourModifierMedium);
            SurfacedDPSH = string.Format("{0:0.00}", raw * ArmourModifierHeavy);
        }
    }
}
