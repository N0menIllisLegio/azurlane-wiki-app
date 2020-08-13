using azurlane_wiki_app.Data.Tables;
using System.ComponentModel;

namespace azurlane_wiki_app.PageEquipmentList.Items
{
    public class SubmarineTorpedo : BaseEquipmentItem
    {
        private int damage;
        private float reload;
        private int sprd;
        private int angle;

        private int bufDamage;
        private float bufReload;

        private string dPSL;
        private string dPSM;
        private string dPSH;

        [DisplayName("Surface DPS\nLight armour")]
        public string DPSL 
        { 
            get => dPSL;
            set
            {
                dPSL = value;
                OnPropertyChanged(nameof(DPSL));
            }
        }
        [DisplayName("Surface DPS\nLight armour")]
        public string DPSM 
        { 
            get => dPSM;
            set
            {
                dPSM = value;
                OnPropertyChanged(nameof(DPSM));
            }
        }
        [DisplayName("Surface DPS\nLight armour")]
        public string DPSH 
        { 
            get => dPSH;
            set 
            { 
                dPSH = value;
                OnPropertyChanged(nameof(DPSH));
            } 
        }

        [DisplayName("Torpedo")]
        public int Torp { get; set; }
        public int Rnd { get; set; }
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
        public int Rng { get; set; }
        public string Sprd { get => $"{sprd}°"; }
        public string Angle { get => $"{angle}°"; }
        public string Attr { get; set; }

        public SubmarineTorpedo(Equipment equipment, DPSData dpsData) : base(equipment)
        {
            Torp = equipment.Torpedo ?? 0;
            Rnd = equipment.Number ?? 0;
            Rng = equipment.WepRange ?? 0;
            sprd = equipment.Spread ?? 0;
            angle = equipment.Angle ?? 0;
            Attr = equipment.Characteristic;

            Damage = equipment.DamageMax ?? 0;
            Reload = equipment.RoFMax ?? 0;
            bufDamage = equipment.Damage ?? 0;
            bufReload = equipment.RoF ?? 0;

            CalcDPS(dpsData);
        }

        private void CalcDPS(DPSData dpsData)
        {
            ArmourModifier armourModifier = dpsData.GetTorpedoArmourModifier(Attr);

            if (armourModifier == null)
            {
                DPSL = DPSM = DPSH = "Error:\nUnknown Attr.";
            }
            else
            {
                float raw = Damage * Rnd / Reload;

                DPSL = string.Format("{0:0.00}", raw * armourModifier.Light);
                DPSM = string.Format("{0:0.00}", raw * armourModifier.Medium);
                DPSH = string.Format("{0:0.00}", raw * armourModifier.Heavy);
            }
        }

        public override void ChangeStats(DPSData dpsData)
        {
            base.ChangeStats(dpsData);

            Damage = Swap(Damage, ref bufDamage);
            Reload = Swap(Reload, ref bufReload);

            CalcDPS(dpsData);
        }
    }
}
