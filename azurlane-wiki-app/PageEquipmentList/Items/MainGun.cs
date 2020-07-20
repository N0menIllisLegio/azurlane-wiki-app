using System;
using System.ComponentModel;
using azurlane_wiki_app.Data.Tables;

namespace azurlane_wiki_app.PageEquipmentList.Items
{
    public class MainGun : BaseEquipmentItem
    {
        public int Firepower { get; set; }
        
        [DisplayName("Anti-Air")]
        public int AA { get; set; }
        public string Rnd { get; set; }
        public int Damage { get; set; }
        public string Coefficient { get; set; }
        public float VT { get; set; }
        public float Reload { get; set; }

        [DisplayName("Rnd/s")]
        public string RndPerS { get; set; }
        public int Rng { get; set; }
        public string Sprd { get; set; }
        public string Angle { get; set; }
        public string Ammo { get; set; }
        public string Attr { get; set; }

        //Surface DPS
        [DisplayName("Rnd\ns")]
        public string Raw { get; set; }
        public string L { get; set; }
        public string M { get; set; }
        public string H { get; set; }

        public MainGun(Equipment equipment, Action<MainGun, double[]> dpsCalc) : base(equipment)
        {
            Firepower = equipment.Firepower ?? 0;
            AA = equipment.AAMax ?? 0;
            Rnd = $"{equipment.Salvoes ?? 0}x{equipment.Shells ?? 0}";
            Damage = equipment.DamageMax ?? 0;
            Coefficient = $"{equipment.Coef ?? 0}%";
            VT = equipment.VolleyTime ?? 0;
            Reload = equipment.RoFMax ?? 0;
            Rng = equipment.WepRange ?? 0;
            Sprd = $"{equipment.Spread ?? 0}°";
            Angle = $"{equipment.Angle ?? 0}°";
            Ammo = equipment.Ammo;
            Attr = equipment.Characteristic;

            if(dpsCalc != null)
            {
                double[] args =
                {
                    Damage,
                    equipment.Coef ?? 0,
                    (equipment.Salvoes ?? 0) * (equipment.Shells ?? 0),
                    Reload,
                    VT
                };

                dpsCalc(this, args);
            }
        }
    }
}
