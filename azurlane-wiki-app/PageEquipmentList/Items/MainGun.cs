using azurlane_wiki_app.Data.Tables;
using System.ComponentModel;

namespace azurlane_wiki_app.PageEquipmentList.Items
{
    public class MainGun : BaseEquipmentItem
    {
        // Buffer for swap
        private int bufDamage;
        private float bufReload;

        private int salvoes;
        private int shells;
        private int coef;
        private float reload;
        private string rndPerS;
        private int damage;
        private int sprd;
        private int angle;

        // DPS
        private string dPSRaw;
        private string dPSL;
        private string dPSM;
        private string dPSH;

        public int Firepower { get; set; }
        public string Rnd { get => $"{salvoes}x{shells}"; }
        public int Damage 
        { 
            get => damage;
            set
            {
                damage = value;
                OnPropertyChanged(nameof(Damage));
            }
        }
        public string Coefficient { get => $"{coef}%"; }
        public float VT { get; set; }
        public float Reload 
        { 
            get => reload;
            set
            {
                reload = value;
                OnPropertyChanged(nameof(Reload));
            }
        }
        [DisplayName("Rnd/s")]
        public string RndPerS 
        { 
            get => rndPerS;
            set
            {
                rndPerS = value;
                OnPropertyChanged(nameof(RndPerS));
            }
        }

        [DisplayName("Surface DPS\nRaw")]
        public string DPSRaw 
        { 
            get => dPSRaw;
            set
            {
                dPSRaw = value;
                OnPropertyChanged(nameof(DPSRaw));
            } 
        }
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
        [DisplayName("Surface DPS\nMedium armour")]
        public string DPSM 
        { 
            get => dPSM;
            set
            {
                dPSM = value;
                OnPropertyChanged(nameof(DPSM));
            }
        }
        [DisplayName("Surface DPS\nHeavy armour")]
        public string DPSH 
        { 
            get => dPSH;
            set
            {
                dPSH = value;
                OnPropertyChanged(nameof(DPSH));
            }
        }

        public int Rng { get; set; }
        public string Sprd { get => $"{sprd}°"; }
        public string Angle { get => $"{angle}°"; }
        public string Ammo { get; set; }
        public string Attr { get; set; }

        public MainGun(Equipment equipment, DPSData dpsData) : base(equipment)
        {
            Firepower = equipment.Firepower ?? 0;
            Rng = equipment.WepRange ?? 0;
            Ammo = equipment.Ammo;
            Attr = equipment.Characteristic;
            VT = equipment.VolleyTime ?? 0;

            coef = equipment.Coef ?? 0;
            shells = equipment.Shells ?? 0;
            salvoes = equipment.Salvoes ?? 0;
            sprd = equipment.Spread ?? 0;
            angle = equipment.Angle ?? 0;

            Damage = equipment.DamageMax ?? 0;
            Reload = equipment.RoFMax ?? 0;

            bufDamage = equipment.Damage ?? 0;
            bufReload = equipment.RoF ?? 0;

            CalcDPS(dpsData);
        }

        private void CalcDPS(DPSData dpsData)
        {
            ArmourModifier ArmourModifiers = dpsData.GetGunArmourModifier(Ammo);

            if (ArmourModifiers == null)
            {
                DPSRaw = DPSL = DPSM = DPSH = RndPerS = "Error:\nUnknown Gun type.";
            }
            else
            {
                double AbsoluteCD = dpsData.Cooldown;
                double raw = Damage * (coef / 100D) * (salvoes * shells) / (Reload + VT + AbsoluteCD);

                DPSRaw = string.Format("{0:0.00}", raw);
                DPSL = string.Format("{0:0.00}", raw * ArmourModifiers.Light);
                DPSM = string.Format("{0:0.00}", raw * ArmourModifiers.Medium);
                DPSH = string.Format("{0:0.00}", raw * ArmourModifiers.Heavy);
                RndPerS = string.Format("{0:0.00}", salvoes * shells / (Reload + VT + AbsoluteCD));
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