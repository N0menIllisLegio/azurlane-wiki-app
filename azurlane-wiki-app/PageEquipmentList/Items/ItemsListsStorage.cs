using azurlane_wiki_app.Data;
using azurlane_wiki_app.Data.Tables;
using System;
using System.Collections.Generic;

namespace azurlane_wiki_app.PageEquipmentList.Items
{
    public struct ArmourModifier
    {
        public double Light;
        public double Medium;
        public double Heavy;
        public double ShellSpeed;
    }

    public class ItemsListsStorage
    {
        private List<MainGun> MainGuns { get; set; } = new List<MainGun>();
        private List<AAGun> AAGuns { get; } = new List<AAGun>();
        private List<Torpedo> Torpedoes { get; } = new List<Torpedo>();
        private List<SubmarineTorpedo> SubmarineTorpedoes { get; } = new List<SubmarineTorpedo>();
        private List<Plane> Planes { get; } = new List<Plane>();
        private List<TorpedoBomberPlane> TorpedoBomberPlanes { get; } = new List<TorpedoBomberPlane>();
        private List<AuxiliaryItem> AuxiliaryItems { get; } = new List<AuxiliaryItem>();
        private List<ASWItem> AswItems { get; } = new List<ASWItem>();

        #region GunsCalcs

        // TODO: Move dictionaries in DB

        private Dictionary<string, Action<MainGun, double[]>> GunsDPSCalcs = new Dictionary<string, Action<MainGun, double[]>> 
        {
            { 
                "DD Gun", 
                (gun, args) => 
                {
                    double AbsoluteCD = 0.26;

                    double dmg = args[0];
                    double coef = args[1] / 100;
                    double numOfShots = args[2];
                    double rld = args[3];
                    double VT = args[4];

                    double raw = (dmg * coef * numOfShots) / (rld + VT + AbsoluteCD);

                    Dictionary<string, ArmourModifier> ArmourModifiers = new Dictionary<string, ArmourModifier>
                    {
                        { "Normal",     new ArmourModifier {Light = 1, Medium = .5, Heavy = .2, ShellSpeed= 180} },
                        { "Normal*",    new ArmourModifier {Light = 1, Medium = .55, Heavy = .25, ShellSpeed= 180} },
                        { "Normal+",    new ArmourModifier {Light = 1, Medium = .6, Heavy = .2, ShellSpeed= 180} },
                        { "HE",         new ArmourModifier {Light = 1.2, Medium = .6, Heavy = .6, ShellSpeed= 150} },
                        { "AP",         new ArmourModifier {Light = .9,  Medium = .7, Heavy = .4, ShellSpeed= 220} },
                    };

                    gun.Raw = string.Format("{0:0.00}", raw);
                    gun.L = string.Format("{0:0.00}", raw * ArmourModifiers[gun.Ammo].Light);
                    gun.M = string.Format("{0:0.00}", raw * ArmourModifiers[gun.Ammo].Medium);
                    gun.H = string.Format("{0:0.00}", raw * ArmourModifiers[gun.Ammo].Heavy);
                    gun.RndPerS = string.Format("{0:0.00}", numOfShots / (rld + VT + AbsoluteCD));
                }
            },
            {
                "CL Gun",
                (gun, args) =>
                {
                    double AbsoluteCD = 0.28;

                    double dmg = args[0];
                    double coef = args[1] / 100;
                    double numOfShots = args[2];
                    double rld = args[3];
                    double VT = args[4];

                    double raw = (dmg * coef * numOfShots) / (rld + VT + AbsoluteCD);

                    Dictionary<string, ArmourModifier> ArmourModifiers = new Dictionary<string, ArmourModifier>
                    {
                        { "Normal", new ArmourModifier {Light = 1, Medium = .75, Heavy = .4, ShellSpeed= 180} },
                        { "HE",     new ArmourModifier {Light = 1.4, Medium = .9, Heavy = .7, ShellSpeed= 150} },
                        { "HE+",    new ArmourModifier {Light = 1.45, Medium = 1.05, Heavy = .7, ShellSpeed= 150} },
                        { "HE++",   new ArmourModifier {Light = 1.45, Medium = 1.1, Heavy = .75, ShellSpeed= 150} },
                        { "AP",     new ArmourModifier {Light = 1,  Medium = .8, Heavy = .6, ShellSpeed= 220} },
                        { "AP+",    new ArmourModifier {Light = 1.1,  Medium = .9, Heavy = .7, ShellSpeed= 220} },
                    };

                    gun.Raw = string.Format("{0:0.00}", raw);
                    gun.L = string.Format("{0:0.00}", raw * ArmourModifiers[gun.Ammo].Light);
                    gun.M = string.Format("{0:0.00}", raw * ArmourModifiers[gun.Ammo].Medium);
                    gun.H = string.Format("{0:0.00}", raw * ArmourModifiers[gun.Ammo].Heavy);
                    gun.RndPerS = string.Format("{0:0.00}", numOfShots / (rld + VT + AbsoluteCD));
                }
            },
            {
                "CA Gun",
                (gun, args) =>
                {
                    double AbsoluteCD = 0.3;

                    double dmg = args[0];
                    double coef = args[1] / 100;
                    double numOfShots = args[2];
                    double rld = args[3];
                    double VT = args[4];

                    double raw = (dmg * coef * numOfShots) / (rld + VT + AbsoluteCD);

                    Dictionary<string, ArmourModifier> ArmourModifiers = new Dictionary<string, ArmourModifier>
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

                    Dictionary<string, double> BurnChance = new Dictionary<string, double>
                    {
                        { "HE",  .08},
                        { "HE*", .09},
                        { "HE+", .04},
                    };

                    gun.Raw = string.Format("{0:0.00}", raw);
                    gun.L = string.Format("{0:0.00}", raw * ArmourModifiers[gun.Ammo].Light);
                    gun.M = string.Format("{0:0.00}", raw * ArmourModifiers[gun.Ammo].Medium);
                    gun.H = string.Format("{0:0.00}", raw * ArmourModifiers[gun.Ammo].Heavy);
                    gun.RndPerS = string.Format("{0:0.00}", numOfShots / (rld + VT + AbsoluteCD));
                }
            },
            {
                "CB Gun",
                (gun, args) =>
                {
                    double AbsoluteCD = 0.3;

                    double dmg = args[0];
                    double coef = args[1] / 100;
                    double numOfShots = args[2];
                    double rld = args[3];
                    double VT = args[4];

                    double raw = (dmg * coef * numOfShots) / (rld + VT + AbsoluteCD);

                    Dictionary<string, ArmourModifier> ArmourModifiers = new Dictionary<string, ArmourModifier>
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

                    Dictionary<string, double> BurnChance = new Dictionary<string, double>
                    {
                        { "HE",  .08},
                        { "HE*", .09},
                        { "HE+", .04},
                    };

                    gun.Raw = string.Format("{0:0.00}", raw);
                    gun.L = string.Format("{0:0.00}", raw * ArmourModifiers[gun.Ammo].Light);
                    gun.M = string.Format("{0:0.00}", raw * ArmourModifiers[gun.Ammo].Medium);
                    gun.H = string.Format("{0:0.00}", raw * ArmourModifiers[gun.Ammo].Heavy);
                    gun.RndPerS = string.Format("{0:0.00}", numOfShots / (rld + VT + AbsoluteCD));
                }
            },
            {
                "BB Gun",
                (gun, args) =>
                {
                    double dmg = args[0];
                    double coef = args[1] / 100;
                    double numOfShots = args[2];
                    double rld = args[3];
                    double VT = args[4];
                    string ammo = gun.Ammo.Equals("Sanshikidan") ? "Type 3 Shell" : gun.Ammo;

                    double raw = (dmg * coef * numOfShots) / (rld + VT);

                    Dictionary<string, ArmourModifier> ArmourModifiers = new Dictionary<string, ArmourModifier>
                    {
                        { "Normal",                     new ArmourModifier {Light = .7, Medium = 1, Heavy = .9, ShellSpeed= 120} },
                        { "HE",                         new ArmourModifier {Light = 1.4, Medium = 1.1, Heavy = .9, ShellSpeed= 100} },
                        { "AP",                         new ArmourModifier {Light = .45,  Medium = 1.3, Heavy = 1.1, ShellSpeed= 120} },
                        { "AP*",                        new ArmourModifier {Light = .4,  Medium = 1.35, Heavy = 1.15, ShellSpeed= 120} },
                        { "AP^",                        new ArmourModifier {Light = .4,  Medium = 1.4, Heavy = 1.2, ShellSpeed= 130} },
                        { "AP+",                        new ArmourModifier {Light = .55,  Medium = 1.45, Heavy = 1.25, ShellSpeed= 120} },
                        { "Type 3 Shell",               new ArmourModifier {Light = 1.4,  Medium = 1.1, Heavy = .9, ShellSpeed= 120} },
                        { "Type 3 Shell Fragments",     new ArmourModifier {Light = 1.45,  Medium = 1.05, Heavy = .7, ShellSpeed= 0} }, //Shell Speed = Random
                        { "Super-Heavy Shell",          new ArmourModifier {Light = .6,  Medium = 1.35, Heavy = 1.15, ShellSpeed= 110} },
                    };

                    Dictionary<string, double> BurnChance = new Dictionary<string, double>
                    {
                        { "AP",  .2},
                        { "AP*", .25},
                        { "AP^", .25},
                        { "AP+", .2},
                    };

                    Dictionary<string, int> SplashRange = new Dictionary<string, int>
                    {
                        { "Normal",                     15},
                        { "HE",                         15},
                        { "AP",                         15},
                        { "AP*",                        15},
                        { "AP^",                        15},
                        { "AP+",                        16},
                        { "Type 3 Shell",               0}, // value at wiki = '-'
                        { "Type 3 Shell Fragments",     0}, // value at wiki = '-'
                        { "Super-Heavy Shell",          8},
                    };
                    // l = 140, m = 110 h = 90

                    gun.Raw = string.Format("{0:0.00}", raw);
                    gun.L = string.Format("{0:0.00}", raw * ArmourModifiers[ammo].Light);
                    gun.M = string.Format("{0:0.00}", raw * ArmourModifiers[ammo].Medium);
                    gun.H = string.Format("{0:0.00}", raw * ArmourModifiers[ammo].Heavy);
                    gun.RndPerS = string.Format("{0:0.00}", numOfShots / (rld + VT));
                }
            }
        };

        

        private Dictionary<string, Action<Plane, Equipment>> PlanesDPSCalcs = new Dictionary<string, Action<Plane, Equipment>>
        {
            {
                "Seaplane",
                (plane, equipment) =>
                {
                    
                }
            }
        };

        #endregion

        private List<string> GetDbTypes(string newType)
        {
            string[,] dictionary =
            {
                { "Anti-Air Guns", "AA Gun" },
                { "Destroyer Guns", "DD Gun" },
                { "Light Cruiser Guns", "CL Gun" },
                { "Battleship Guns", "BB Gun" },
                { "Heavy Cruiser Guns", "CA Gun" },
                { "Large Cruiser Guns", "CB Gun" },
                { "Heavy Cruiser Guns", "SS Gun" },
                { "Submarine Torpedoes", "Submarine Torpedo" },
                { "Anti-Submarine Equipment", "ASW Helicopter" },
                { "Anti-Submarine Equipment", "ASW Bomber" },
                { "Anti-Submarine Equipment", "Sonar" },
                { "Anti-Submarine Equipment", "Depth Charge" },
                { "Seaplanes", "Seaplane" },
                { "Torpedo Bomber Planes", "Torpedo Bomber" },
                { "Ship Torpedoes", "Torpedo" },
                { "Fighter Planes", "Fighter" },
                { "Dive Bomber Planes", "Dive Bomber" },
                { "Auxiliary Equipment", "Auxiliary" }
            };

            List<string> dbTypes = new List<string>();

            for (int i = 0; i < dictionary.GetLength(0); i++)
            {
                if (dictionary[i, 0] == newType)
                {
                    dbTypes.Add(dictionary[i, 1]);
                }
            }

            return dbTypes;
        }

        public object GetList(string newType)
        {
            switch (GetDbTypes(newType)[0])
            {
                case "AA Gun":
                    return AAGuns;
                case "Auxiliary":
                    return AuxiliaryItems;
                case "Submarine Torpedo":
                    return SubmarineTorpedoes;
                case "Torpedo":
                    return Torpedoes;
                case "Torpedo Bomber":
                    return TorpedoBomberPlanes;
                case string s when s.Contains("Gun"):
                    return MainGuns;
                case string s when s == "Fighter" || s == "Dive Bomber" || s == "Seaplane":
                    return Planes;
                default:
                    return AswItems;
            }
        }

        public void FillList(string newTypeFullName)
        {
            List<string> dbTypes = GetDbTypes(newTypeFullName);

            using (CargoContext cargoContext = new CargoContext())
            {
                List<Equipment> equipment = new List<Equipment>();

                foreach (string dbType in dbTypes)
                {
                    var temp = cargoContext.EquipmentTypes.Find(dbType)?.EquipmentList;

                    if (temp != null)
                    {
                        equipment.AddRange(temp);
                    }
                }

                foreach (Equipment equip in equipment)
                {
                    AddItemToList(equip, equip.FK_Type.Name);
                }
            }
        }

        private void AddItemToList(Equipment item, string newType)
        {
            switch (newType)
            {
                case "AA Gun":
                    AAGuns.Add(new AAGun(item));
                    break;
                case "Auxiliary":
                    AuxiliaryItems.Add(new AuxiliaryItem(item));
                    break;
                case "Submarine Torpedo":
                    SubmarineTorpedoes.Add(new SubmarineTorpedo(item));
                    break;
                case "Torpedo":
                    Torpedoes.Add(new Torpedo(item));
                    break;
                case "Torpedo Bomber":
                    TorpedoBomberPlanes.Add(new TorpedoBomberPlane(item));
                    break;
                case string s when s.Contains("Gun"):

                    Action<MainGun, double[]> calc = null;

                    if (GunsDPSCalcs.ContainsKey(s))
                    {
                        calc = GunsDPSCalcs[s];
                    }

                    MainGuns.Add(new MainGun(item, calc));
                    break;
                case string s when s == "Fighter" || s == "Dive Bomber" || s == "Seaplane":
                    Planes.Add(new Plane(item));
                    break;
                default:
                    AswItems.Add(new ASWItem(item));
                    break;
            }
        }

        public void ClearLists()
        {
            //MainGuns = null;
            //MainGuns = new List<MainGun>();

            AAGuns.Clear();
            AuxiliaryItems.Clear();
            SubmarineTorpedoes.Clear();
            Torpedoes.Clear();
            TorpedoBomberPlanes.Clear();
            MainGuns.Clear();
            Planes.Clear();
            AswItems.Clear();
        }
    }
}
