using System;
using System.Collections.Generic;
using System.ComponentModel;
using azurlane_wiki_app.Data.Tables;

namespace azurlane_wiki_app.PageEquipmentList.Items
{
    public class SubmarineTorpedo : BaseEquipmentItem
    {
        private int damage;
        private float reload;
        private int bufDamage;
        private float bufReload;
        private string dPSL;
        private string dPSM;
        private string dPSH;

        //Surface DPS
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
        public string Sprd { get; set; }
        public string Angle { get; set; }
        public string Attr { get; set; }

        public SubmarineTorpedo(Equipment equipment) : base(equipment)
        {
            Torp = equipment.Torpedo ?? 0;
            Rnd = equipment.Number ?? 0;

            Damage = equipment.DamageMax ?? 0;
            Reload = equipment.RoFMax ?? 0;
            bufDamage = equipment.Damage ?? 0;
            bufReload = equipment.RoF ?? 0;

            Rng = equipment.WepRange ?? 0;
            Sprd = $"{equipment.Spread ?? 0}°";
            Angle = $"{equipment.Angle ?? 0}°";
            Attr = equipment.Characteristic;

            CalcDPS();
        }

        private void CalcDPS()
        {
            Dictionary<string, ArmourModifier> ArmourModifiers = new Dictionary<string, ArmourModifier>
            {
                { "Normal", new ArmourModifier {Light = .8, Medium = 1, Heavy = 1.3, ShellSpeed = 0 } },
                { "Magnetic", new ArmourModifier {Light = .8, Medium = 1, Heavy = 1.3, ShellSpeed = 0 } },
                { "Oxygen (Type 95)", new ArmourModifier {Light = .8, Medium = 1, Heavy = 1.3, ShellSpeed = 0 } }
            };

            Dictionary<string, int> TorpedoSpeed = new Dictionary<string, int>
            {
                { "Normal", 30 },
                { "Magnetic", 20 },
                { "Oxygen (Type 95)", 30 } // + accelirating
            };

            string ammo = Attr;

            if (!ArmourModifiers.ContainsKey(ammo))
            {
                ammo = "Normal";
            }

            ArmourModifier armourModifier = ArmourModifiers[ammo];

            float raw = Damage * Rnd / Reload;

            DPSL = string.Format("{0:0.00}", raw * armourModifier.Light);
            DPSM = string.Format("{0:0.00}", raw * armourModifier.Medium);
            DPSH = string.Format("{0:0.00}", raw * armourModifier.Heavy);
        }

        public override void ChangeStats()
        {
            base.ChangeStats();
            Damage = Swap(Damage, ref bufDamage);
            Reload = Swap(Reload, ref bufReload);
            CalcDPS();
        }
    }
}
