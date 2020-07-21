using azurlane_wiki_app.Data.Tables;
using System.Collections.Generic;
using System.ComponentModel;

namespace azurlane_wiki_app.PageEquipmentList.Items
{
    public class Plane : BaseEquipmentItem
    {
        protected struct BombDamageLevels
        {
            public int Dmg;
            public int Dmg3;
            public int Dmg6;
            public int DmgMax;
        }

        protected struct BombStats
        {
            public Dictionary<string, BombDamageLevels> Damages;

            public double ArmourModifierLight;
            public double ArmourModifierMedium;
            public double ArmourModifierHeavy;

            public int SplashRange;
        }

        struct GunStats
        {
            public double Rld;
            public double Rld3;
            public double Rld6;
            public double RldMax;

            public int Dmg;
            public int Dmg3;
            public int Dmg6;
            public int DmgMax;
        }

        public int Aviation { get; set; }
        public string Ordnance { get; set; }
        public string Guns { get; set; }
        public float Rld { get; set; }

        [DisplayName("Surfaced DPS\nLight armour")]
        public string SurfacedDPSL { get; set; }
        [DisplayName("Surfaced DPS\nMedium armour")]
        public string SurfacedDPSM { get; set; }
        [DisplayName("Surfaced DPS\nHeavy armour")]
        public string SurfacedDPSH { get; set; }
        [DisplayName("Anti-Air\nDPS")]
        public string AADPS { get; set; }
        [DisplayName("Anti-Air\nBurst")]
        public string AADPSBurst { get; set; }

        public Plane(Equipment equipment) : base(equipment)
        {
            Aviation = equipment.Aviation ?? 0;
            Ordnance = $"{equipment.Bombs1}\n{equipment.Bombs2}".Trim();
            Guns = $"{equipment.AAGun1}\n{equipment.AAGun2}".Trim();
            Rld = equipment.RoFMax ?? 0;

            CalcBombsDPS(equipment);
            CalcAAGunsDPS(equipment.AAGun1, equipment.AAGun2);
        }

        protected (int count, BombStats type) ParseBombs(string str)
        {
            Dictionary<string, BombStats> Bombs = new Dictionary<string, BombStats>
            {
                // 0 == N/A
                {
                    "100lb", new BombStats
                    {
                        ArmourModifierLight = .8, ArmourModifierMedium = .85, ArmourModifierHeavy = 1, SplashRange = 16,
                        Damages = new Dictionary<string, BombDamageLevels>
                        {
                            {"T0", new BombDamageLevels {Dmg =  74, Dmg3 = 107, Dmg6 = 140, DmgMax = 185}},
                            {"T1", new BombDamageLevels {Dmg =  57, Dmg3 =  81, Dmg6 = 108, DmgMax = 144}},
                            {"T2", new BombDamageLevels {Dmg =  63, Dmg3 =  90, Dmg6 = 117, DmgMax = 159}},
                            {"T3", new BombDamageLevels {Dmg =  69, Dmg3 =  99, Dmg6 = 131, DmgMax = 173}},
                        }
                    }
                },
                {
                    "500lb VF-17", new BombStats
                    {
                        ArmourModifierLight = .8, ArmourModifierMedium = .9, ArmourModifierHeavy = 1.1, SplashRange = 19,
                        Damages = new Dictionary<string, BombDamageLevels>
                        {
                            {"T0", new BombDamageLevels {Dmg = 152, Dmg3 = 219, Dmg6 = 288, DmgMax = 380}}
                        }
                    }
                },
                {
                    "500lb", new BombStats
                    {
                        ArmourModifierLight = .8, ArmourModifierMedium = .9, ArmourModifierHeavy = 1.1, SplashRange = 19,
                        Damages = new Dictionary<string, BombDamageLevels>
                        {
                            {"T0", new BombDamageLevels {Dmg = 144, Dmg3 = 207, Dmg6 = 270, DmgMax = 360}},
                            {"T1", new BombDamageLevels {Dmg = 120, Dmg3 = 174, Dmg6 = 228, DmgMax = 300}},
                            {"T2", new BombDamageLevels {Dmg = 132, Dmg3 = 191, Dmg6 = 249, DmgMax = 330}},
                            {"T3", new BombDamageLevels {Dmg = 144, Dmg3 = 207, Dmg6 = 270, DmgMax = 360}},
                        }
                    }
                },
                {
                    "1000lb", new BombStats
                    {
                        ArmourModifierLight = .8, ArmourModifierMedium = .95, ArmourModifierHeavy = 1.15, SplashRange = 22,
                        Damages = new Dictionary<string, BombDamageLevels>
                        {
                            {"T0", new BombDamageLevels {Dmg = 161, Dmg3 = 233, Dmg6 = 305, DmgMax = 402}},
                            {"T1", new BombDamageLevels {Dmg = 134, Dmg3 = 192, Dmg6 = 251, DmgMax = 335}},
                            {"T2", new BombDamageLevels {Dmg = 149, Dmg3 = 212, Dmg6 = 279, DmgMax = 369}},
                            {"T3", new BombDamageLevels {Dmg = 161, Dmg3 = 233, Dmg6 = 305, DmgMax = 402}},
                        }
                    }
                },
                {
                    "1600lb AP", new BombStats
                    {
                        ArmourModifierLight = .8, ArmourModifierMedium = 1.1, ArmourModifierHeavy = 1.3, SplashRange = 25,
                        Damages = new Dictionary<string, BombDamageLevels>
                        {
                            {"T0", new BombDamageLevels {Dmg = 171, Dmg3 = 247, Dmg6 = 325, DmgMax = 429}}
                        }
                    }
                },
                {
                    "1600lb", new BombStats
                    {
                        ArmourModifierLight = .75, ArmourModifierMedium = 1, ArmourModifierHeavy = 1.2, SplashRange = 25,
                        Damages = new Dictionary<string, BombDamageLevels>
                        {
                            {"T0", new BombDamageLevels {Dmg = 171, Dmg3 = 247, Dmg6 = 325, DmgMax = 429}},
                            {"T1", new BombDamageLevels {Dmg = 142, Dmg3 = 206, Dmg6 = 269, DmgMax = 357}},
                            {"T2", new BombDamageLevels {Dmg = 157, Dmg3 = 226, Dmg6 = 297, DmgMax = 393}},
                            {"T3", new BombDamageLevels {Dmg = 171, Dmg3 = 247, Dmg6 = 325, DmgMax = 429}},
                        }
                    }
                },

                {
                    "2000lb", new BombStats
                    {
                        ArmourModifierLight = .7, ArmourModifierMedium = 1.05, ArmourModifierHeavy = 1.25, SplashRange = 28,
                        Damages = new Dictionary<string, BombDamageLevels>
                        {
                            {"T0", new BombDamageLevels {Dmg = 192, Dmg3 = 277, Dmg6 = 364, DmgMax = 480}},
                            {"T1", new BombDamageLevels {Dmg = 153, Dmg3 = 221, Dmg6 = 288, DmgMax = 380}},
                            {"T2", new BombDamageLevels {Dmg = 167, Dmg3 = 239, Dmg6 = 315, DmgMax = 417}},
                            {"T3", new BombDamageLevels {Dmg = 182, Dmg3 = 263, Dmg6 = 344, DmgMax = 456}},
                        }
                    }
                }
            };

            var result = (count: 0, type: Bombs["100lb"]);

            if (string.IsNullOrEmpty(str))
            {
                return result;
            }

            if (!int.TryParse(str.Split('x')[0].Trim(), out result.count))
            {
                return result;
            }

            //string[] splittedStr = str.Trim().Split(' ');

            foreach(string key in Bombs.Keys)
            {
                if (str.Contains(key))
                {
                    result.type = Bombs[key];
                    break;
                }
            }

            return result;
        }

        /// armourmod; 0 = Light, 1 = Medium, 2 = Heavy
        protected double CalcBombDPSNumerator(int BombsCount, BombStats BombsType, byte armourmod)
        {
            double armmod = .0;
            switch (armourmod)
            {
                case 0:
                    armmod = BombsType.ArmourModifierLight;
                    break;
                case 1:
                    armmod = BombsType.ArmourModifierMedium;
                    break;
                case 2:
                    armmod = BombsType.ArmourModifierHeavy;
                    break;
            }

            int damage;
            switch (Stars)
            {
                case int stars when stars <= 2:
                    damage = BombsType.Damages[Tech].Dmg3;
                    break;
                case 3:
                    damage = BombsType.Damages[Tech].Dmg6;
                    break;
                default:
                    damage = BombsType.Damages[Tech].DmgMax;
                    break;
            }

            return damage * armmod * BombsCount;
        }

        protected virtual void CalcBombsDPS(Equipment equipment)
        {
            double AbsoluteCD = .1;
            double denominator = Rld * 2.2 + AbsoluteCD;

            var Bombs1 = ParseBombs(equipment.Bombs1);
            var Bombs2 = ParseBombs(equipment.Bombs2);

            
            SurfacedDPSL = string.Format("{0:0.00}", (CalcBombDPSNumerator(Bombs1.count, Bombs1.type, 0)
                + CalcBombDPSNumerator(Bombs2.count, Bombs2.type, 0)) / denominator);

            SurfacedDPSM = string.Format("{0:0.00}", (CalcBombDPSNumerator(Bombs1.count, Bombs1.type, 1)
                + CalcBombDPSNumerator(Bombs2.count, Bombs2.type, 1)) / denominator);

            SurfacedDPSH = string.Format("{0:0.00}", (CalcBombDPSNumerator(Bombs1.count, Bombs1.type, 2)
                + CalcBombDPSNumerator(Bombs2.count, Bombs2.type, 2)) / denominator);

        }

        private (double damage, double reload) CalcAAGunNumerator(string gun)
        {
            var result = (damage: .0, reload: 1.0);

            if (string.IsNullOrEmpty(gun.Trim()))
            {
                return result;
            }

            Dictionary<string, Dictionary<string, GunStats>> Guns = new Dictionary<string, Dictionary<string, GunStats>>
            {
                {
                    "2 x 7.7mm MG GL.2", new Dictionary<string,GunStats>
                    {
                        { "T0", new GunStats {Rld= 75, Rld3= 70, Rld6= 66, RldMax= 60, Dmg=  9, Dmg3= 12, Dmg6= 17, DmgMax= 22} },
                    }
                },
                {
                    "2 x 7.7mm MG", new Dictionary<string,GunStats>
                    {
                        { "T1", new GunStats {Rld= 79, Rld3= 74, Rld6= 70, RldMax= 63, Dmg=  8, Dmg3= 11, Dmg6= 16, DmgMax= 20} },
                        { "T2", new GunStats {Rld= 75, Rld3= 70, Rld6= 66, RldMax= 60, Dmg=  9, Dmg3= 12, Dmg6= 17, DmgMax= 22} },
                        { "T3", new GunStats {Rld= 72, Rld3= 67, Rld6= 62, RldMax= 57, Dmg= 10, Dmg3= 13, Dmg6= 18, DmgMax= 26} },
                        { "T0", new GunStats {Rld= 72, Rld3= 67, Rld6= 62, RldMax= 57, Dmg= 10, Dmg3= 13, Dmg6= 18, DmgMax= 26} },
                    }
                },
                {
                    "3 x 7.7mm MG", new Dictionary<string,GunStats>
                    {
                        { "T0", new GunStats {Rld= 72, Rld3= 67, Rld6= 62, RldMax= 57, Dmg= 14, Dmg3= 18, Dmg6= 24, DmgMax= 34} },
                    }
                },
                {
                    "4 x 7.7mm MG", new Dictionary<string,GunStats>
                    {
                        { "T1", new GunStats {Rld= 79, Rld3= 74, Rld6= 70, RldMax= 63, Dmg= 13, Dmg3= 18, Dmg6= 23, DmgMax= 32} },
                        { "T2", new GunStats {Rld= 75, Rld3= 70, Rld6= 66, RldMax= 60, Dmg= 14, Dmg3= 19, Dmg6= 27, DmgMax= 35} },
                        { "T3", new GunStats {Rld= 72, Rld3= 67, Rld6= 62, RldMax= 57, Dmg= 16, Dmg3= 22, Dmg6= 30, DmgMax= 38} },
                    }
                },
                {
                    "8 x 7.7mm MG", new Dictionary<string,GunStats>
                    {
                        { "T1", new GunStats {Rld= 79, Rld3= 74, Rld6= 70, RldMax= 63, Dmg= 21, Dmg3= 29, Dmg6= 37, DmgMax= 51} },
                        { "T2", new GunStats {Rld= 75, Rld3= 70, Rld6= 66, RldMax= 60, Dmg= 22, Dmg3= 30, Dmg6= 43, DmgMax= 56} },
                        { "T3", new GunStats {Rld= 72, Rld3= 67, Rld6= 62, RldMax= 57, Dmg= 26, Dmg3= 35, Dmg6= 48, DmgMax= 61} },
                    }
                },
                {
                    "2 x 7.92mm MG17 MG", new Dictionary<string,GunStats>
                    {
                        { "T1", new GunStats {Rld= 68, Rld3= 65, Rld6= 61, RldMax= 53, Dmg= 10, Dmg3= 13, Dmg6= 16, DmgMax= 21} },
                        { "T2", new GunStats {Rld= 64, Rld3= 61, Rld6= 57, RldMax= 49, Dmg= 11, Dmg3= 14, Dmg6= 17, DmgMax= 22} },
                        { "T3", new GunStats {Rld= 60, Rld3= 57, Rld6= 53, RldMax= 45, Dmg= 13, Dmg3= 16, Dmg6= 19, DmgMax= 24} },
                    }
                },
                {
                    "2 x 7.92mm MG", new Dictionary<string,GunStats>
                    {
                        { "T1", new GunStats {Rld= 68, Rld3= 65, Rld6= 61, RldMax= 53, Dmg= 10, Dmg3= 13, Dmg6= 16, DmgMax= 21} },
                        { "T2", new GunStats {Rld= 64, Rld3= 61, Rld6= 57, RldMax= 49, Dmg= 11, Dmg3= 14, Dmg6= 17, DmgMax= 22} },
                        { "T3", new GunStats {Rld= 60, Rld3= 57, Rld6= 53, RldMax= 45, Dmg= 13, Dmg3= 16, Dmg6= 19, DmgMax= 24} },
                    }
                },
                {
                    "3 x 7.92mm MG", new Dictionary<string,GunStats>
                    {
                        { "T1", new GunStats {Rld= 76, Rld3= 73, Rld6= 68, RldMax= 60, Dmg= 15, Dmg3= 18, Dmg6= 21, DmgMax= 26} },
                        { "T2", new GunStats {Rld= 72, Rld3= 69, Rld6= 65, RldMax= 57, Dmg= 16, Dmg3= 19, Dmg6= 22, DmgMax= 27} },
                        { "T3", new GunStats {Rld= 69, Rld3= 66, Rld6= 62, RldMax= 54, Dmg= 18, Dmg3= 21, Dmg6= 24, DmgMax= 29} },
                    }
                },
                {
                    "2 x 12.7mm MG", new Dictionary<string,GunStats>
                    {
                        { "T1", new GunStats {Rld= 93, Rld3= 87, Rld6= 81, RldMax= 74, Dmg= 11, Dmg3= 15, Dmg6= 19, DmgMax= 27} },
                        { "T2", new GunStats {Rld= 88, Rld3= 83, Rld6= 77, RldMax= 70, Dmg= 12, Dmg3= 16, Dmg6= 20, DmgMax= 28} },
                        { "T3", new GunStats {Rld= 83, Rld3= 78, Rld6= 74, RldMax= 67, Dmg= 14, Dmg3= 19, Dmg6= 24, DmgMax= 32} },
                        { "T0", new GunStats {Rld= 83, Rld3= 78, Rld6= 74, RldMax= 67, Dmg= 14, Dmg3= 19, Dmg6= 24, DmgMax= 32} },
                    }
                },
                {
                    "4 x 12.7mm MG", new Dictionary<string,GunStats>
                    {
                        { "T1", new GunStats {Rld=133, Rld3=124, Rld6=116, RldMax=105, Dmg= 18, Dmg3= 24, Dmg6= 30, DmgMax= 38} },
                        { "T2", new GunStats {Rld=126, Rld3=118, Rld6=110, RldMax=100, Dmg= 20, Dmg3= 26, Dmg6= 33, DmgMax= 42} },
                        { "T3", new GunStats {Rld=119, Rld3=112, Rld6=105, RldMax= 95, Dmg= 22, Dmg3= 28, Dmg6= 36, DmgMax= 46} },
                        { "T0", new GunStats {Rld=114, Rld3=107, Rld6=100, RldMax= 90, Dmg= 31, Dmg3= 41, Dmg6= 53, DmgMax= 69} },
                    }
                },
                {
                    "6 x 12.7mm MG", new Dictionary<string,GunStats>
                    {
                        { "T1", new GunStats {Rld=170, Rld3=159, Rld6=149, RldMax=136, Dmg= 22, Dmg3= 33, Dmg6= 43, DmgMax= 58} },
                        { "T2", new GunStats {Rld=162, Rld3=152, Rld6=142, RldMax=129, Dmg= 25, Dmg3= 36, Dmg6= 47, DmgMax= 63} },
                        { "T3", new GunStats {Rld=154, Rld3=145, Rld6=135, RldMax=122, Dmg= 27, Dmg3= 39, Dmg6= 51, DmgMax= 69} },
                        { "T0", new GunStats {Rld=146, Rld3=137, Rld6=128, RldMax=115, Dmg= 31, Dmg3= 43, Dmg6= 58, DmgMax= 78} },
                    }
                },
                {
                    "1 x 13mm MG", new Dictionary<string,GunStats>
                    {
                        { "T1", new GunStats {Rld= 93, Rld3= 87, Rld6= 81, RldMax= 74, Dmg=  6, Dmg3=  8, Dmg6= 10, DmgMax= 14} },
                        { "T2", new GunStats {Rld= 88, Rld3= 83, Rld6= 77, RldMax= 70, Dmg=  6, Dmg3=  9, Dmg6= 12, DmgMax= 16} },
                        { "T3", new GunStats {Rld= 83, Rld3= 78, Rld6= 74, RldMax= 67, Dmg=  8, Dmg3= 11, Dmg6= 14, DmgMax= 18} },
                        { "T0", new GunStats {Rld= 83, Rld3= 78, Rld6= 74, RldMax= 67, Dmg=  8, Dmg3= 11, Dmg6= 14, DmgMax= 18} },
                    }
                },
                {
                    "2 x 13mm MG131 MG", new Dictionary<string,GunStats>
                    {
                        { "T1", new GunStats {Rld= 98, Rld3= 92, Rld6= 86, RldMax= 79, Dmg= 12, Dmg3= 16, Dmg6= 20, DmgMax= 28} },
                        { "T2", new GunStats {Rld= 93, Rld3= 88, Rld6= 82, RldMax= 75, Dmg= 12, Dmg3= 16, Dmg6= 20, DmgMax= 28} },
                        { "T3", new GunStats {Rld= 88, Rld3= 83, Rld6= 79, RldMax= 72, Dmg= 14, Dmg3= 19, Dmg6= 24, DmgMax= 32} },
                    }
                },
                {
                    "2 x 20mm Cannon", new Dictionary<string,GunStats>
                    {
                        { "T1", new GunStats {Rld=134, Rld3=125, Rld6=117, RldMax=107, Dmg= 13, Dmg3= 19, Dmg6= 25, DmgMax= 34} },
                        { "T2", new GunStats {Rld=127, Rld3=119, Rld6=112, RldMax=102, Dmg= 14, Dmg3= 21, Dmg6= 28, DmgMax= 37} },
                        { "T3", new GunStats {Rld=120, Rld3=113, Rld6=107, RldMax= 97, Dmg= 15, Dmg3= 23, Dmg6= 30, DmgMax= 40} },
                        { "T0", new GunStats {Rld=120, Rld3=113, Rld6=107, RldMax= 97, Dmg= 15, Dmg3= 23, Dmg6= 30, DmgMax= 40} },
                    }
                },
                {
                    "4 x 20mm Cannon", new Dictionary<string,GunStats>
                    {
                        { "T1", new GunStats {Rld=187, Rld3=176, Rld6=165, RldMax=149, Dmg= 27, Dmg3= 39, Dmg6= 51, DmgMax= 68} },
                        { "T2", new GunStats {Rld=178, Rld3=167, Rld6=157, RldMax=142, Dmg= 30, Dmg3= 43, Dmg6= 56, DmgMax= 74} },
                        { "T3", new GunStats {Rld=169, Rld3=158, Rld6=149, RldMax=135, Dmg= 33, Dmg3= 45, Dmg6= 60, DmgMax= 80} },
                        { "T0", new GunStats {Rld=160, Rld3=151, Rld6=141, RldMax=128, Dmg= 36, Dmg3= 51, Dmg6= 66, DmgMax= 86} },
                    }
                },
                {
                    "2 x 20mm MG FF Cannon", new Dictionary<string,GunStats>
                    {
                        { "T1", new GunStats {Rld=101, Rld3= 95, Rld6= 89, RldMax= 82, Dmg= 14, Dmg3= 20, Dmg6= 26, DmgMax= 35} },
                        { "T2", new GunStats {Rld= 96, Rld3= 91, Rld6= 85, RldMax= 78, Dmg= 17, Dmg3= 23, Dmg6= 29, DmgMax= 38} },
                        { "T3", new GunStats {Rld= 91, Rld3= 86, Rld6= 82, RldMax= 75, Dmg= 20, Dmg3= 26, Dmg6= 32, DmgMax= 41} },
                    }
                },
                {
                    "3 x 20mm MG151 Cannon", new Dictionary<string,GunStats>
                    {
                        { "T1", new GunStats {Rld=160, Rld3=151, Rld6=143, RldMax=133, Dmg= 20, Dmg3= 29, Dmg6= 38, DmgMax= 50} },
                        { "T2", new GunStats {Rld=153, Rld3=145, Rld6=138, RldMax=128, Dmg= 25, Dmg3= 34, Dmg6= 43, DmgMax= 55} },
                        { "T3", new GunStats {Rld=146, Rld3=139, Rld6=133, RldMax=123, Dmg= 30, Dmg3= 39, Dmg6= 48, DmgMax= 60} },
                    }
                },
                {
                    "4 x 20mm Type-99 Cannon", new Dictionary<string,GunStats>
                    {
                        { "T1", new GunStats {Rld=187, Rld3=176, Rld6=165, RldMax=149, Dmg= 29, Dmg3= 41, Dmg6= 53, DmgMax= 69} },
                        { "T2", new GunStats {Rld=178, Rld3=167, Rld6=157, RldMax=142, Dmg= 32, Dmg3= 44, Dmg6= 57, DmgMax= 77} },
                        { "T3", new GunStats {Rld=169, Rld3=158, Rld6=149, RldMax=135, Dmg= 35, Dmg3= 50, Dmg6= 65, DmgMax= 85} },
                    }
                },
            };

            switch (Stars)
            {
                case int stars when stars <= 2:
                    result.damage = Guns[gun][Tech].Dmg3;
                    result.reload = Guns[gun][Tech].Rld3;
                    break;
                case 3:
                    result.damage = Guns[gun][Tech].Dmg6;
                    result.reload = Guns[gun][Tech].Rld6;
                    break;
                default:
                    result.damage = Guns[gun][Tech].DmgMax;
                    result.reload = Guns[gun][Tech].RldMax;
                    break;
            }


            return result;
        }

        protected void CalcAAGunsDPS(string equipGun1, string equipGun2)
        {
            double AbsoluteCD = .1;
            double magickNumber = 150.359569; // WIKI
            double denominator = Rld * 2.2 + AbsoluteCD;

            string AAGun1 = !string.IsNullOrEmpty(equipGun1) ? equipGun1.Remove(equipGun1.Length - 2, 2).Trim() : "";
            string AAGun2 = !string.IsNullOrEmpty(equipGun2) ? equipGun2.Remove(equipGun2.Length - 2, 2).Trim() : "";


            var AAGun1Result = CalcAAGunNumerator(AAGun1);
            var AAGun2Result = CalcAAGunNumerator(AAGun2);

            AADPS = string.Format("{0:0.00}", ((AAGun1Result.damage * (magickNumber / AAGun1Result.reload)) + (AAGun2Result.damage * (magickNumber / AAGun2Result.reload))) / denominator);
            AADPSBurst = string.Format("{0:0.00}", (AAGun1Result.damage + AAGun2Result.damage) / denominator);
        }
    }
}
