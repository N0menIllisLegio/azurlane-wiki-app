using azurlane_wiki_app.Data.Tables;
using System.ComponentModel;

namespace azurlane_wiki_app.PageEquipmentList.Items
{
    public class Plane : BaseEquipmentItem
    {
        private float rld;
        protected float bufRld;

        private string bombs1;
        private string bombs2;
        private string aaGun1;
        private string aaGun2;
        private string surfacedDPSL;
        private string surfacedDPSM;
        private string surfacedDPSH;
        private string aADPS;
        private string aADPSBurst;

        public int Aviation { get; set; }
        public string Ordnance { get => $"{bombs1}\n{bombs2}".Trim(); }
        public string Guns { get => $"{aaGun1}\n{aaGun2}".Trim(); }
        public float Rld
        {
            get => rld;
            set
            {
                rld = value;
                OnPropertyChanged(nameof(Rld));
            }
        }

        #region DPS
        [DisplayName("Surface DPS\nLight armour")]
        public string SurfacedDPSL 
        { 
            get => surfacedDPSL;
            set
            {
                surfacedDPSL = value;
                OnPropertyChanged(nameof(SurfacedDPSL));
            }
        }
        [DisplayName("Surface DPS\nMedium armour")]
        public string SurfacedDPSM 
        { 
            get => surfacedDPSM;
            set
            {
                surfacedDPSM = value;
                OnPropertyChanged(nameof(SurfacedDPSM));
            }
        }
        [DisplayName("Surface DPS\nHeavy armour")]
        public string SurfacedDPSH 
        { 
            get => surfacedDPSH;
            set
            {
                surfacedDPSH = value;
                OnPropertyChanged(nameof(SurfacedDPSH));
            }
        }
        [DisplayName("Anti-Air\nDPS")]
        public string AADPS 
        { 
            get => aADPS;
            set
            {
                aADPS = value;
                OnPropertyChanged(nameof(AADPS));
            }
        }
        [DisplayName("Anti-Air\nBurst")]
        public string AADPSBurst 
        { 
            get => aADPSBurst;
            set
            {
                aADPSBurst = value;
                OnPropertyChanged(nameof(AADPSBurst));
            }
        }
        #endregion

        public Plane(Equipment equipment, DPSData dpsData) : base(equipment)
        {
            Aviation = equipment.Aviation ?? 0;

            bombs1 = equipment.Bombs1;
            bombs2 = equipment.Bombs2;
            aaGun1 = equipment.AAGun1;
            aaGun2 = equipment.AAGun2;

            Rld = equipment.RoFMax ?? 0;
            bufRld = equipment.RoF ?? 0;

            CalcDPSes(dpsData);
        }

        // Important: Cooldown for aaguns and bombs is taken from same source.

        protected (int count, BombStats type) ParseBombs(string str, DPSData dpsData)
        {
            var result = (count: 0, type: (BombStats) null);

            if (string.IsNullOrEmpty(str))
            {
                return result;
            }

            if (!int.TryParse(str.Split('x')[0].Trim(), out result.count))
            {
                return result;
            }

            result.type = dpsData.GetPlaneBombStats(str);

            return result;
        }

        /// armourmod; 0 = Light, 1 = Medium, 2 = Heavy
        protected double CalcBombDPSNumerator(int BombsCount, BombStats BombsType, byte armourmod)
        {
            if (BombsCount <= 0)
            {
                return 0;
            }

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

            if (DisplayMaxStats)
            {
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
            }
            else
            {
                damage = BombsType.Damages[Tech].Dmg;
            }

            return damage * armmod * BombsCount;
        }

        protected virtual void CalcBombsDPS(int TorpNumber, int TorpDamage, DPSData dpsData)
        {
            double AbsoluteCD = dpsData.Cooldown;
            double denominator = Rld * 2.2 + AbsoluteCD;

            var Bombs1 = ParseBombs(bombs1, dpsData);
            var Bombs2 = ParseBombs(bombs2, dpsData);

            if ((Bombs1.count > 0 && Bombs1.type == null) || (Bombs2.count > 0 && Bombs2.type == null))
            {
                SurfacedDPSL = SurfacedDPSM = SurfacedDPSH = "Error:\nUnknown Bombs.";
            }
            else
            {
                SurfacedDPSL = string.Format("{0:0.00}", (CalcBombDPSNumerator(Bombs1.count, Bombs1.type, 0)
                + CalcBombDPSNumerator(Bombs2.count, Bombs2.type, 0)) / denominator);

                SurfacedDPSM = string.Format("{0:0.00}", (CalcBombDPSNumerator(Bombs1.count, Bombs1.type, 1)
                    + CalcBombDPSNumerator(Bombs2.count, Bombs2.type, 1)) / denominator);

                SurfacedDPSH = string.Format("{0:0.00}", (CalcBombDPSNumerator(Bombs1.count, Bombs1.type, 2)
                    + CalcBombDPSNumerator(Bombs2.count, Bombs2.type, 2)) / denominator);
            }
        }

        private (double damage, double reload) CalcAAGunNumerator(string gun, DPSData dpsData)
        {
            var result = (damage: .0, reload: 1.0);

            if (string.IsNullOrEmpty(gun.Trim()))
            {
                return result;
            }

            GunStats gunStats = dpsData.GetPlaneGunStats(gun, Tech);

            if (DisplayMaxStats)
            {
                switch (Stars)
                {
                    case int stars when stars <= 2:
                        result.damage = gunStats.Dmg3;
                        result.reload = gunStats.Rld3;
                        break;
                    case 3:
                        result.damage = gunStats.Dmg6;
                        result.reload = gunStats.Rld6;
                        break;
                    default:
                        result.damage = gunStats.DmgMax;
                        result.reload = gunStats.RldMax;
                        break;
                }
            }
            else
            {
                result.damage = gunStats.Dmg;
                result.reload = gunStats.Rld;
            }

            return result;
        }

        protected void CalcAAGunsDPS(DPSData dpsData)
        {
            double AbsoluteCD = dpsData.Cooldown;
            double magicNumber = 150.359569; // WIKI
            double denominator = Rld * 2.2 + AbsoluteCD;

            string AAGun1 = !string.IsNullOrEmpty(aaGun1) ? aaGun1.Remove(aaGun1.Length - 2, 2).Trim() : "";
            string AAGun2 = !string.IsNullOrEmpty(aaGun2) ? aaGun2.Remove(aaGun2.Length - 2, 2).Trim() : "";

            var AAGun1Result = CalcAAGunNumerator(AAGun1, dpsData);
            var AAGun2Result = CalcAAGunNumerator(AAGun2, dpsData);

            AADPS = string.Format("{0:0.00}", ((AAGun1Result.damage * (magicNumber / AAGun1Result.reload)) + (AAGun2Result.damage * (magicNumber / AAGun2Result.reload))) / denominator);
            AADPSBurst = string.Format("{0:0.00}", (AAGun1Result.damage + AAGun2Result.damage) / denominator);
        }

        public override void ChangeStats(DPSData dpsData)
        {
            base.ChangeStats(dpsData);

            Rld = Swap(Rld, ref bufRld);

            CalcDPSes(dpsData);
        }

        protected virtual void CalcDPSes(DPSData dpsData)
        {
            CalcBombsDPS(0, 0, dpsData);
            CalcAAGunsDPS(dpsData);
        }
    }
}
