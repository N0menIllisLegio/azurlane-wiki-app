using azurlane_wiki_app.Data.Tables;
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

        public Torpedo(Equipment equipment, DPSData dpsData) : base(equipment, dpsData)
        {
            CalcDPS(dpsData);
        }

        private void CalcDPS(DPSData dpsData)
        {
            ArmourModifier armourModifier = dpsData.GetTorpedoArmourModifier(Attr);

            if (armourModifier == null)
            {
                PreloadDPSL = PreloadDPSM = PreloadDPSH = "Error:\nUnknown Attr.";
            }
            else
            {
                double raw = Damage * Rnd / 60.0;
                double l = Damage * Rnd * armourModifier.Light / Reload;
                double m = Damage * Rnd * armourModifier.Medium / Reload;
                double h = Damage * Rnd * armourModifier.Heavy / Reload;

                PreloadDPSL = string.Format("{0:0.00}", l + raw * armourModifier.Light);
                PreloadDPSM = string.Format("{0:0.00}", m + raw * armourModifier.Medium);
                PreloadDPSH = string.Format("{0:0.00}", h + raw * armourModifier.Heavy);
            }
        }

        public override void ChangeStats(DPSData dpsData)
        {
            base.ChangeStats(dpsData);

            CalcDPS(dpsData);
        }
    }
}
