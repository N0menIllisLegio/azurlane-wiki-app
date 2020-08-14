using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.IO;
using System.Threading.Tasks;
using azurlane_wiki_app.Data.Downloaders;
using azurlane_wiki_app.Data.Tables;
using azurlane_wiki_app.PageEquipmentList.Items;
using SQLite.CodeFirst;

namespace azurlane_wiki_app.Data
{
    class CargoContext : DbContext
    {
        public DbSet<ShipGirl> ShipGirls { get; set; }
        public DbSet<Equipment> ShipGirlsEquipment { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<WhereToGetShipGirl> WhereToGetShipGirls { get; set; }
        public DbSet<ShipGirlWhereToGetShipGirl> ShipGirlWhereToGetShipGirl { get; set; }
        public DbSet<Icon> Icons { get; set; }
        public DbSet<Rarity> Rarities { get; set; }
        public DbSet<Nationality> Nationalities { get; set; }
        public DbSet<ShipGroup> ShipGroups { get; set; }
        public DbSet<ShipClass> ShipClasses { get; set; }
        public DbSet<ShipType> ShipTypes { get; set; }
        public DbSet<EquipmentTech> EquipmentTeches { get; set; }
        public DbSet<EquipmentType> EquipmentTypes { get; set; }

        public DbSet<ArmourModifier> ArmourModifiers { get; set; }
        public DbSet<GunStats> GunsStats { get; set; }
        public DbSet<BombStats> BombsStats { get; set; }

        public CargoContext() : base("CargoConnection") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new SqliteCreateDatabaseIfNotExists<CargoContext>(modelBuilder));
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
        }

        /// <summary>
        /// Clears all tables (ShipGirls, Equipment, Skills, WhereToGetShipGirls, ShipGirlsWhereToGetShipGirls).
        /// </summary>
        public void ClearTables()
        {
            this.Database.ExecuteSqlCommand("DELETE FROM [ShipGirls]");
            this.Database.ExecuteSqlCommand("DELETE FROM [Equipment]");
            this.Database.ExecuteSqlCommand("DELETE FROM [Skills]");
            this.Database.ExecuteSqlCommand("DELETE FROM [WhereToGetShipGirls]");
            this.Database.ExecuteSqlCommand("DELETE FROM [ShipGirlWhereToGetShipGirls]");
        }

        public void ClearDPSDataTables()
        {
            this.Database.ExecuteSqlCommand("DELETE FROM [ArmourModifiers]");
            this.Database.ExecuteSqlCommand("DELETE FROM [GunsStats]");
            this.Database.ExecuteSqlCommand("DELETE FROM [BombsStats]");
        }

        /// <summary>
        /// Removes all images.
        /// </summary>
        public void ClearImages()
        {
            Directory.Delete(DataDownloader.ImagesFolderPath, true);
        }

        /// <summary>
        /// Removes file on path if exist
        /// </summary>
        /// <param name="path">Path to file</param>
        private void DeleteImage(string path)
        {
            if (path != null && File.Exists(path))
            {
                File.Delete(path);
            }
        }

        /// <summary>
        /// Updates data of Ship Girl. (Also removes Ship Girl's images)
        /// </summary>
        /// <param name="newShipGirl">New Ship Girl data</param>
        public async Task Update(ShipGirl newShipGirl)
        {
            ShipGirl oldShipGirl = await ShipGirls.FindAsync(newShipGirl.ShipID);

            if (oldShipGirl !=  null)
            {
                DeleteImage(oldShipGirl.Image);
                DeleteImage(oldShipGirl.ImageBanner);
                DeleteImage(oldShipGirl.ImageBannerKai);
                DeleteImage(oldShipGirl.ImageChibi);
                DeleteImage(oldShipGirl.ImageChibiKai);
                DeleteImage(oldShipGirl.ImageIcon);
                DeleteImage(oldShipGirl.ImageIconKai);
                DeleteImage(oldShipGirl.ImageKai);
                DeleteImage(oldShipGirl.ImageShipyardIcon);
                DeleteImage(oldShipGirl.ImageShipyardIconKai);
                 
                #region Fuck Ship Girls

                oldShipGirl.AA120 = newShipGirl.AA120;
                oldShipGirl.AAInitial = newShipGirl.AAInitial;
                oldShipGirl.AAKai = newShipGirl.AAKai;
                oldShipGirl.AAKai120 = newShipGirl.AAKai120;
                oldShipGirl.AAMax = newShipGirl.AAMax;
                oldShipGirl.Acc120 = newShipGirl.Acc120;
                oldShipGirl.AccInitial = newShipGirl.AccInitial;
                oldShipGirl.AccKai = newShipGirl.AccKai;
                oldShipGirl.AccKai120 = newShipGirl.AccKai120;
                oldShipGirl.AccMax = newShipGirl.AccMax;
                oldShipGirl.Air120 = newShipGirl.Air120;
                oldShipGirl.AirInitial = newShipGirl.AirInitial;
                oldShipGirl.AirKai = newShipGirl.AirKai;
                oldShipGirl.AirKai120 = newShipGirl.AirKai120;
                oldShipGirl.AirMax = newShipGirl.AirMax;
                oldShipGirl.Ammo120 = newShipGirl.Ammo120;
                oldShipGirl.AmmoInitial = newShipGirl.AmmoInitial;
                oldShipGirl.AmmoKai = newShipGirl.AmmoKai;
                oldShipGirl.AmmoKai120 = newShipGirl.AmmoKai120;
                oldShipGirl.AmmoMax = newShipGirl.AmmoMax;
                oldShipGirl.Armor = newShipGirl.Armor;
                oldShipGirl.ArmorKai = newShipGirl.ArmorKai;
                oldShipGirl.ASW120 = newShipGirl.ASW120;
                oldShipGirl.ASWInitial = newShipGirl.ASWInitial;
                oldShipGirl.ASWKai = newShipGirl.ASWKai;
                oldShipGirl.ASWKai120 = newShipGirl.ASWKai120;
                oldShipGirl.ASWMax = newShipGirl.ASWMax;
                oldShipGirl.ConstructTime = newShipGirl.ConstructTime;
                oldShipGirl.Consumption120 = newShipGirl.Consumption120;
                oldShipGirl.ConsumptionInitial = newShipGirl.ConsumptionInitial;
                oldShipGirl.ConsumptionKai = newShipGirl.ConsumptionKai;
                oldShipGirl.ConsumptionKai120 = newShipGirl.ConsumptionKai120;
                oldShipGirl.ConsumptionMax = newShipGirl.ConsumptionMax;
                oldShipGirl.Eq1EffInit = newShipGirl.Eq1EffInit;
                oldShipGirl.Eq1EffInitKai = newShipGirl.Eq1EffInitKai;
                oldShipGirl.Eq1EffInitMax = newShipGirl.Eq1EffInitMax;
                oldShipGirl.Eq2EffInit = newShipGirl.Eq2EffInit;
                oldShipGirl.Eq2EffInitKai = newShipGirl.Eq2EffInitKai;
                oldShipGirl.Eq2EffInitMax = newShipGirl.Eq2EffInitMax;
                oldShipGirl.Eq3EffInit = newShipGirl.Eq3EffInit;
                oldShipGirl.Eq3EffInitKai = newShipGirl.Eq3EffInitKai;
                oldShipGirl.Eq3EffInitMax = newShipGirl.Eq3EffInitMax;
                oldShipGirl.Evade120 = newShipGirl.Evade120;
                oldShipGirl.EvadeInitial = newShipGirl.EvadeInitial;
                oldShipGirl.EvadeKai = newShipGirl.EvadeKai;
                oldShipGirl.EvadeKai120 = newShipGirl.EvadeKai120;
                oldShipGirl.EvadeMax = newShipGirl.EvadeMax;
                oldShipGirl.Fire120 = newShipGirl.Fire120;
                oldShipGirl.FireInitial = newShipGirl.FireInitial;
                oldShipGirl.FireKai = newShipGirl.FireKai;
                oldShipGirl.FireKai120 = newShipGirl.FireKai120;
                oldShipGirl.FireMax = newShipGirl.FireMax;
                oldShipGirl.Health120 = newShipGirl.Health120;
                oldShipGirl.HealthInitial = newShipGirl.HealthInitial;
                oldShipGirl.HealthKai = newShipGirl.HealthKai;
                oldShipGirl.HealthKai120 = newShipGirl.HealthKai120;
                oldShipGirl.HealthMax = newShipGirl.HealthMax;
                oldShipGirl.Image = newShipGirl.Image;
                oldShipGirl.ImageBanner = newShipGirl.ImageBanner;
                oldShipGirl.ImageBannerKai = newShipGirl.ImageBannerKai;
                oldShipGirl.ImageChibi = newShipGirl.ImageChibi;
                oldShipGirl.ImageChibiKai = newShipGirl.ImageChibiKai;
                oldShipGirl.ImageIcon = newShipGirl.ImageIcon;
                oldShipGirl.ImageIconKai = newShipGirl.ImageIconKai;
                oldShipGirl.ImageKai = newShipGirl.ImageKai;
                oldShipGirl.ImageShipyardIcon = newShipGirl.ImageShipyardIcon;
                oldShipGirl.ImageShipyardIconKai = newShipGirl.ImageShipyardIconKai;
                oldShipGirl.LimitBreak1 = newShipGirl.LimitBreak1;
                oldShipGirl.LimitBreak2 = newShipGirl.LimitBreak2;
                oldShipGirl.LimitBreak3 = newShipGirl.LimitBreak3;
                oldShipGirl.Luck = newShipGirl.Luck;
                oldShipGirl.Name = newShipGirl.Name;
                oldShipGirl.Oxygen120 = newShipGirl.Oxygen120;
                oldShipGirl.OxygenInitial = newShipGirl.OxygenInitial;
                oldShipGirl.OxygenKai = newShipGirl.OxygenKai;
                oldShipGirl.OxygenKai120 = newShipGirl.OxygenKai120;
                oldShipGirl.OxygenMax = newShipGirl.OxygenMax;
                oldShipGirl.Reload120 = newShipGirl.Reload120;
                oldShipGirl.ReloadInitial = newShipGirl.ReloadInitial;
                oldShipGirl.ReloadKai = newShipGirl.ReloadKai;
                oldShipGirl.ReloadKai120 = newShipGirl.ReloadKai120;
                oldShipGirl.ReloadMax = newShipGirl.ReloadMax;
                oldShipGirl.Remodel = newShipGirl.Remodel;
                oldShipGirl.Speed = newShipGirl.Speed;
                oldShipGirl.SpeedKai = newShipGirl.SpeedKai;
                oldShipGirl.Torp120 = newShipGirl.Torp120;
                oldShipGirl.TorpInitial = newShipGirl.TorpInitial;
                oldShipGirl.TorpKai = newShipGirl.TorpKai;
                oldShipGirl.TorpKai120 = newShipGirl.TorpKai120;
                oldShipGirl.TorpMax = newShipGirl.TorpMax;

                oldShipGirl.FK_Nationality = newShipGirl.FK_Nationality;
                oldShipGirl.FK_Rarity = newShipGirl.FK_Rarity;
                oldShipGirl.FK_ShipGroup = newShipGirl.FK_ShipGroup;
                oldShipGirl.FK_SubtypeRetro = newShipGirl.FK_SubtypeRetro;
                oldShipGirl.FK_ShipType = newShipGirl.FK_ShipType;
                oldShipGirl.FK_ShipClass = newShipGirl.FK_ShipClass;

                oldShipGirl.FK_Eq1Type = newShipGirl.FK_Eq1Type;
                oldShipGirl.FK_Eq2Type = newShipGirl.FK_Eq2Type;
                oldShipGirl.FK_Eq3Type = newShipGirl.FK_Eq3Type;

                #endregion

                await SaveChangesAsync();
            }
        }

        /// <summary>
        /// Updates data of one piece of Equipment. (Also removes it image)
        /// </summary>
        /// <param name="newEquipment">New Equipment data</param>
        public async Task Update(Equipment newEquipment)
        {
            Equipment oldEquipment = await ShipGirlsEquipment.FirstOrDefaultAsync(e => e.Name == newEquipment.Name);

            if (oldEquipment != null)
            {
                DeleteImage(oldEquipment.Image);

                #region Fuck Equipment

                oldEquipment.AA = newEquipment.AA;
                oldEquipment.AAGun1 = newEquipment.AAGun1;
                oldEquipment.AAGun2 = newEquipment.AAGun2;
                oldEquipment.AAMax = newEquipment.AAMax;
                oldEquipment.Acc = newEquipment.Acc;
                oldEquipment.AccMax = newEquipment.AccMax;
                oldEquipment.Ammo = newEquipment.Ammo;
                oldEquipment.Angle = newEquipment.Angle;
                oldEquipment.AR = newEquipment.AR;
                oldEquipment.ARNote = newEquipment.ARNote;
                oldEquipment.ASW = newEquipment.ASW;
                oldEquipment.ASWMax = newEquipment.ASWMax;
                oldEquipment.Aviation = newEquipment.Aviation;
                oldEquipment.AvMax = newEquipment.AvMax;
                oldEquipment.BB = newEquipment.BB;
                oldEquipment.BBNote = newEquipment.BBNote;
                oldEquipment.BBV = newEquipment.BBV;
                oldEquipment.BBVNote = newEquipment.BBVNote;
                oldEquipment.BC = newEquipment.BC;
                oldEquipment.BCNote = newEquipment.BCNote;
                oldEquipment.BM = newEquipment.BM;
                oldEquipment.BMNote = newEquipment.BMNote;
                oldEquipment.Bombs1 = newEquipment.Bombs1;
                oldEquipment.Bombs2 = newEquipment.Bombs2;
                oldEquipment.CA = newEquipment.CA;
                oldEquipment.CANote = newEquipment.CANote;
                oldEquipment.CB = newEquipment.CB;
                oldEquipment.CBNote = newEquipment.CBNote;
                oldEquipment.Characteristic = newEquipment.Characteristic;
                oldEquipment.CL = newEquipment.CL;
                oldEquipment.CLNote = newEquipment.CLNote;
                oldEquipment.Coef = newEquipment.Coef;
                oldEquipment.CV = newEquipment.CV;
                oldEquipment.CVL = newEquipment.CVL;
                oldEquipment.CVLNote = newEquipment.CVLNote;
                oldEquipment.CVNote = newEquipment.CVNote;
                oldEquipment.Damage = newEquipment.Damage;
                oldEquipment.DamageMax = newEquipment.DamageMax;
                oldEquipment.DD = newEquipment.DD;
                oldEquipment.DDNote = newEquipment.DDNote;
                oldEquipment.DropLocation = newEquipment.DropLocation;
                oldEquipment.Evasion = newEquipment.Evasion;
                oldEquipment.EvasionMax = newEquipment.EvasionMax;
                oldEquipment.Firepower = newEquipment.Firepower;
                oldEquipment.FPMax = newEquipment.FPMax;
                oldEquipment.Health = newEquipment.Health;
                oldEquipment.HealthMax = newEquipment.HealthMax;
                oldEquipment.Image = newEquipment.Image;
                oldEquipment.Luck = newEquipment.Luck;
                oldEquipment.LuckMax = newEquipment.LuckMax;
                oldEquipment.Name = newEquipment.Name;
                oldEquipment.Notes = newEquipment.Notes;
                oldEquipment.Number = newEquipment.Number;
                oldEquipment.Oxygen = newEquipment.Oxygen;
                oldEquipment.OxygenMax = newEquipment.OxygenMax;
                oldEquipment.PingFreq = newEquipment.PingFreq;
                oldEquipment.PlaneHP = newEquipment.PlaneHP;
                oldEquipment.PlaneHPMax = newEquipment.PlaneHPMax;
                oldEquipment.Reload = newEquipment.Reload;
                oldEquipment.ReloadMax = newEquipment.ReloadMax;
                oldEquipment.RoF = newEquipment.RoF;
                oldEquipment.RoFMax = newEquipment.RoFMax;
                oldEquipment.Salvoes = newEquipment.Salvoes;
                oldEquipment.Shells = newEquipment.Shells;
                oldEquipment.Spd = newEquipment.Spd;
                oldEquipment.SpdMax = newEquipment.SpdMax;
                oldEquipment.Spread = newEquipment.Spread;
                oldEquipment.SS = newEquipment.SS;
                oldEquipment.SSNote = newEquipment.SSNote;
                oldEquipment.SSV = newEquipment.SSV;
                oldEquipment.SSVNote = newEquipment.SSVNote;
                oldEquipment.Stars = newEquipment.Stars;
                oldEquipment.Torpedo = newEquipment.Torpedo;
                oldEquipment.TorpMax = newEquipment.TorpMax;
                oldEquipment.VolleyTime = newEquipment.VolleyTime;
                oldEquipment.WepRange = newEquipment.WepRange;

                oldEquipment.FK_Nationality = newEquipment.FK_Nationality;
                oldEquipment.FK_Tech = newEquipment.FK_Tech;
                oldEquipment.FK_Type = newEquipment.FK_Type;

                #endregion

                await SaveChangesAsync();
            }
        }

        /// <summary>
        /// Updates data of one Skill. (Also removes it icon)
        /// </summary>
        /// <param name="newSkill">New Skill data</param>
        public async Task Update(Skill newSkill)
        {
            Skill oldSkill = await Skills.FirstOrDefaultAsync(e => e.Name == newSkill.Name);

            if (oldSkill != null)
            {
                DeleteImage(oldSkill.Icon);

                oldSkill.Num = newSkill.Num;
                oldSkill.Name = newSkill.Name;
                oldSkill.Type = newSkill.Type;
                oldSkill.Detail = newSkill.Detail;
                oldSkill.Remodel = newSkill.Remodel;
                oldSkill.Icon = newSkill.Icon;

                oldSkill.FK_ShipGirl = newSkill.FK_ShipGirl;

                await SaveChangesAsync();
            }
        }
    }
}
