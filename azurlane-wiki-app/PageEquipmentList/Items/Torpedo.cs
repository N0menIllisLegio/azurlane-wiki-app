using azurlane_wiki_app.Data.Tables;
using System.Collections.Generic;
using System.ComponentModel;

namespace azurlane_wiki_app.PageEquipmentList.Items
{
    public class Torpedo : SubmarineTorpedo
    {
        private string preloadDPSL;
        private string preloadDPSM;
        private string preloadDPSH;

        //Preload DPS
        [DisplayName("Preload DPS\nLight armour")]
        public string PreloadDPSL 
        { 
            get => preloadDPSL;
            set
            {
                preloadDPSL = value;
                OnPropertyChanged(nameof(PreloadDPSL));
            }
        }
        [DisplayName("Preload DPS\nMedium armour")]
        public string PreloadDPSM 
        { 
            get => preloadDPSM;
            set
            {
                preloadDPSM = value;
                OnPropertyChanged(nameof(PreloadDPSM));
            }
        }
        [DisplayName("Preload DPS\nHeavy armour")]
        public string PreloadDPSH 
        { 
            get => preloadDPSH;
            set
            {
                preloadDPSH = value;
                OnPropertyChanged(nameof(PreloadDPSH));
            }
        }

        public Torpedo(Equipment equipment) : base(equipment)
        {
            CalcDPS();
        }

        private void CalcDPS()
        {
            Dictionary<string, ArmourModifier> ArmourModifiers = new Dictionary<string, ArmourModifier>
            {
                { "Normal", new ArmourModifier {Light = .8, Medium = 1, Heavy = 1.3, ShellSpeed = 0 } },
                { "Magnetic", new ArmourModifier {Light = .8, Medium = 1, Heavy = 1.3, ShellSpeed = 0 } },
                { "Oxygen (Sakura)", new ArmourModifier {Light = .8, Medium = 1, Heavy = 1.3, ShellSpeed = 0 } }
            };

            Dictionary<string, int> TorpedoSpeed = new Dictionary<string, int>
            {
                { "Normal", 30 },
                { "Magnetic", 20 },
                { "Oxygen (Sakura)", 40 }
            };

            string ammo = Attr;

            if (!ArmourModifiers.ContainsKey(ammo))
            {
                ammo = "Normal";
            }

            ArmourModifier armourModifier = ArmourModifiers[ammo];

            double raw = Damage * Rnd / 60.0;
            double l = Damage * Rnd * armourModifier.Light / Reload;
            double m = Damage * Rnd * armourModifier.Medium / Reload;
            double h = Damage * Rnd * armourModifier.Heavy / Reload;

            PreloadDPSL = string.Format("{0:0.00}", l + raw * armourModifier.Light);
            PreloadDPSM = string.Format("{0:0.00}", m + raw * armourModifier.Medium);
            PreloadDPSH = string.Format("{0:0.00}", h + raw * armourModifier.Heavy);
        }

        public override void ChangeStats()
        {
            base.ChangeStats();
            CalcDPS();
        }
    }
}
