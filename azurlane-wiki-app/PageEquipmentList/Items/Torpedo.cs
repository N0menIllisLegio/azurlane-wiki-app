using azurlane_wiki_app.Data.Tables;
using System.Collections.Generic;

namespace azurlane_wiki_app.PageEquipmentList.Items
{
    public class Torpedo : SubmarineTorpedo
    {
        //Preload DPS

        public string PreloadL { get; set; }
        public string PreloadM { get; set; }
        public string PreloadH { get; set; }

        public Torpedo(Equipment equipment) : base(equipment)
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

            PreloadL = string.Format("{0:0.00}", l + raw * armourModifier.Light);
            PreloadM = string.Format("{0:0.00}", m + raw * armourModifier.Medium);
            PreloadH = string.Format("{0:0.00}", h + raw * armourModifier.Heavy);
        }
    }
}
