using azurlane_wiki_app.Data.Tables;
using System.Collections.Generic;
using System.ComponentModel;

namespace azurlane_wiki_app.PageEquipmentList.Items
{
    public class MainGun : BaseEquipmentItem
    {
        private int bufDamage;
        private float bufReload;
        private float reload;
        private string rndPerS;
        private int damage;
        private string dPSRaw;
        private string dPSL;
        private string dPSM;
        private string dPSH;

        private int salvoes;
        private int shells;
        private int coef;

        private string gunType;

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

        //Surface DPS
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
        public string Sprd { get; set; }
        public string Angle { get; set; }
        public string Ammo { get; set; }
        public string Attr { get; set; }

        public MainGun(Equipment equipment) : base(equipment)
        {
            Firepower = equipment.Firepower ?? 0;
            Rng = equipment.WepRange ?? 0;
            Sprd = $"{equipment.Spread ?? 0}°";
            Angle = $"{equipment.Angle ?? 0}°";
            Ammo = equipment.Ammo;
            Attr = equipment.Characteristic;
            VT = equipment.VolleyTime ?? 0;

            coef = equipment.Coef ?? 0;
            shells = equipment.Shells ?? 0;
            salvoes = equipment.Salvoes ?? 0;
            
            Damage = equipment.DamageMax ?? 0;
            Reload = equipment.RoFMax ?? 0;
            bufDamage = equipment.Damage ?? 0;
            bufReload = equipment.RoF ?? 0;

            gunType = equipment.Type;

            CalcDPS();
        }

        private void CalcDPS()
        {
            Dictionary<string, ArmourModifier> ArmourModifiers = null;
            double AbsoluteCD = 0;
            string ammo = Ammo;

            switch (gunType)
            {
                case "DD Gun":
                    AbsoluteCD = 0.26;
                    ArmourModifiers = new Dictionary<string, ArmourModifier>
                    {
                        { "Normal",     new ArmourModifier {Light = 1, Medium = .5, Heavy = .2, ShellSpeed= 180} },
                        { "Normal*",    new ArmourModifier {Light = 1, Medium = .55, Heavy = .25, ShellSpeed= 180} },
                        { "Normal+",    new ArmourModifier {Light = 1, Medium = .6, Heavy = .2, ShellSpeed= 180} },
                        { "HE",         new ArmourModifier {Light = 1.2, Medium = .6, Heavy = .6, ShellSpeed= 150} },
                        { "AP",         new ArmourModifier {Light = .9,  Medium = .7, Heavy = .4, ShellSpeed= 220} },
                    };

                    break;
                case "CL Gun":
                    AbsoluteCD = 0.28;
                    ArmourModifiers = new Dictionary<string, ArmourModifier>
                    {
                        { "Normal", new ArmourModifier {Light = 1, Medium = .75, Heavy = .4, ShellSpeed= 180} },
                        { "HE",     new ArmourModifier {Light = 1.4, Medium = .9, Heavy = .7, ShellSpeed= 150} },
                        { "HE+",    new ArmourModifier {Light = 1.45, Medium = 1.05, Heavy = .7, ShellSpeed= 150} },
                        { "HE++",   new ArmourModifier {Light = 1.45, Medium = 1.1, Heavy = .75, ShellSpeed= 150} },
                        { "AP",     new ArmourModifier {Light = 1,  Medium = .8, Heavy = .6, ShellSpeed= 220} },
                        { "AP+",    new ArmourModifier {Light = 1.1,  Medium = .9, Heavy = .7, ShellSpeed= 220} },
                    };

                    break;
                case "SS Gun": // Submarine-mounted Twin 203mm: Surcouf only.
                case "CA Gun":
                case "CB Gun":
                    AbsoluteCD = 0.3;
                    ArmourModifiers = new Dictionary<string, ArmourModifier>
                    {
                        { "Normal",     new ArmourModifier {Light = 1, Medium = .9, Heavy = .5, ShellSpeed= 180} },
                        { "NormalPR",   new ArmourModifier {Light = 1.15, Medium = 1.1, Heavy = .9, ShellSpeed= 200} },
                        { "NormalDR",   new ArmourModifier {Light = 1.15, Medium = 1.15, Heavy = .95, ShellSpeed= 200} },
                        { "HE",         new ArmourModifier {Light = 1.35, Medium = .95, Heavy = .7, ShellSpeed= 160} },
                        { "HE*",        new ArmourModifier {Light = 1.35, Medium = .95, Heavy = .7, ShellSpeed= 180} },
                        { "HE+",        new ArmourModifier {Light = 1.35, Medium = 1, Heavy = .75, ShellSpeed= 120} },
                        { "AP",         new ArmourModifier {Light = .75,  Medium = 1.1, Heavy = .75, ShellSpeed= 220} },
                        { "SAP",        new ArmourModifier {Light = .65,  Medium = 1.25, Heavy = .65, ShellSpeed= 220} },
                    };

                    break;
                case "BB Gun":
                    ammo = ammo.Equals("Sanshikidan") ? "Type 3 Shell" : Ammo;
                    ArmourModifiers = new Dictionary<string, ArmourModifier>
                    {
                        { "Normal",                 new ArmourModifier {Light = .7, Medium = 1, Heavy = .9, ShellSpeed= 120} },
                        { "HE",                     new ArmourModifier {Light = 1.4, Medium = 1.1, Heavy = .9, ShellSpeed= 100} },
                        { "AP",                     new ArmourModifier {Light = .45,  Medium = 1.3, Heavy = 1.1, ShellSpeed= 120} },
                        { "AP*",                    new ArmourModifier {Light = .4,  Medium = 1.35, Heavy = 1.15, ShellSpeed= 120} },
                        { "AP^",                    new ArmourModifier {Light = .4,  Medium = 1.4, Heavy = 1.2, ShellSpeed= 130} },
                        { "AP+",                    new ArmourModifier {Light = .55,  Medium = 1.45, Heavy = 1.25, ShellSpeed= 120} },
                        { "Type 3 Shell",           new ArmourModifier {Light = 1.4,  Medium = 1.1, Heavy = .9, ShellSpeed= 120} },
                        { "Type 3 Shell Fragments", new ArmourModifier {Light = 1.45,  Medium = 1.05, Heavy = .7, ShellSpeed= 0} }, //Shell Speed = Random
                        { "Super-Heavy Shell",      new ArmourModifier {Light = .6,  Medium = 1.35, Heavy = 1.15, ShellSpeed= 110} },
                    };

                    break;
            }

            double raw = Damage * (coef / 100D) * (salvoes * shells) / (Reload + VT + AbsoluteCD);

            if (ArmourModifiers == null)
            {
                DPSRaw = DPSL = DPSM = DPSH = RndPerS = "Error:\nUnknown Gun type.";
            }
            else
            {
                DPSRaw = string.Format("{0:0.00}", raw);
                DPSL = string.Format("{0:0.00}", raw * ArmourModifiers?[ammo].Light);
                DPSM = string.Format("{0:0.00}", raw * ArmourModifiers?[ammo].Medium);
                DPSH = string.Format("{0:0.00}", raw * ArmourModifiers?[ammo].Heavy);
                RndPerS = string.Format("{0:0.00}", salvoes * shells / (Reload + VT + AbsoluteCD));
            }
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

// CA Gun

//Dictionary<string, double> BurnChance = new Dictionary<string, double>
//{
//    { "HE",  .08},
//    { "HE*", .09},
//    { "HE+", .04},
//};

//// CB Gun

//Dictionary<string, double> BurnChance = new Dictionary<string, double>
//{
//    { "HE",  .08},
//    { "HE*", .09},
//    { "HE+", .04},
//};

//// BB Gun

//Dictionary<string, double> BurnChance = new Dictionary<string, double>
//{
//    { "AP",  .2},
//    { "AP*", .25},
//    { "AP^", .25},
//    { "AP+", .2},
//};

//Dictionary<string, int> SplashRange = new Dictionary<string, int>
//{
//    { "Normal",                     15},
//    { "HE",                         15},
//    { "AP",                         15},
//    { "AP*",                        15},
//    { "AP^",                        15},
//    { "AP+",                        16},
//    { "Type 3 Shell",               0}, // value at wiki = '-'
//    { "Type 3 Shell Fragments",     0}, // value at wiki = '-'
//    { "Super-Heavy Shell",          8},
//};