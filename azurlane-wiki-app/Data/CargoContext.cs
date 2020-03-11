using System;
using azurlane_wiki_app.Data.Downloaders;
using azurlane_wiki_app.Data.Tables;
using SQLite.CodeFirst;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace azurlane_wiki_app
{
    class CargoContext : DbContext
    {
        public DbSet<ShipGirl> ShipGirls { get; set; }
        public DbSet<Equipment> ShipGirlsEquipment { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<WhereToGetShipGirl> WhereToGetShipGirls { get; set; }
        public DbSet<ShipGirlWhereToGetShipGirl> ShipGirlWhereToGetShipGirl { get; set; }

        public CargoContext() : base("CargoConnection") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer(new SqliteCreateDatabaseIfNotExists<CargoContext>(modelBuilder));
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
        }

        public async Task CreateRelationshipGirlDrop(WhereToGetShipGirl wtg, ShipGirl shipGirl, string note)
        {
            ShipGirlWhereToGetShipGirl mtm = new ShipGirlWhereToGetShipGirl
            {
                FK_ShipGirl = shipGirl,
                FK_WhereToGetShipGirl = wtg,
                Note = note
            };

            if (ShipGirlWhereToGetShipGirl.Count(e => e.FK_ShipGirl.ShipID == mtm.FK_ShipGirl.ShipID && 
                                                      e.FK_WhereToGetShipGirl.Name == mtm.FK_WhereToGetShipGirl.Name) == 0)
            {
                ShipGirlWhereToGetShipGirl.Add(mtm);
                await SaveChangesAsync();
            }
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

        /// <summary>
        /// Removes all images.
        /// </summary>
        public void ClearImages()
        {
            Directory.Delete(DataDownloader.ImagesFolderPath, true);
        }

        private void DeleteImage(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        /// <summary>
        /// Updates data of Ship Girl. (Also removes Ship Girl's images)
        /// </summary>
        /// <param name="newShipGirl">Ship Girl for update</param>
        /// <returns></returns>
        public async Task Update(ShipGirl newShipGirl)
        {
            ShipGirl oldShipGirl = await ShipGirls.FindAsync(newShipGirl.ShipID);

            if (oldShipGirl !=  null)
            {
                ShipDownloader shipDownloader = new ShipDownloader();

                DeleteImage(shipDownloader.GetImageFolder(oldShipGirl.Image) + "/" + oldShipGirl.Image);
                DeleteImage(shipDownloader.GetImageFolder(oldShipGirl.ImageBanner) + "/" + oldShipGirl.ImageBanner);
                DeleteImage(shipDownloader.GetImageFolder(oldShipGirl.ImageBannerKai) + "/" + oldShipGirl.ImageBannerKai);
                DeleteImage(shipDownloader.GetImageFolder(oldShipGirl.ImageChibi) + "/" + oldShipGirl.ImageChibi);
                DeleteImage(shipDownloader.GetImageFolder(oldShipGirl.ImageChibiKai) + "/" + oldShipGirl.ImageChibiKai);
                DeleteImage(shipDownloader.GetImageFolder(oldShipGirl.ImageIcon) + "/" + oldShipGirl.ImageIcon);
                DeleteImage(shipDownloader.GetImageFolder(oldShipGirl.ImageIconKai) + "/" + oldShipGirl.ImageIconKai);
                DeleteImage(shipDownloader.GetImageFolder(oldShipGirl.ImageKai) + "/" + oldShipGirl.ImageKai);
                DeleteImage(shipDownloader.GetImageFolder(oldShipGirl.ImageShipyardIcon) + "/" + oldShipGirl.ImageShipyardIcon);
                DeleteImage(shipDownloader.GetImageFolder(oldShipGirl.ImageShipyardIconKai) + "/" + oldShipGirl.ImageShipyardIconKai);
                    
                #region Fuck it

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
                oldShipGirl.Class = newShipGirl.Class;
                oldShipGirl.ConstructTime = newShipGirl.ConstructTime;
                oldShipGirl.Consumption120 = newShipGirl.Consumption120;
                oldShipGirl.ConsumptionInitial = newShipGirl.ConsumptionInitial;
                oldShipGirl.ConsumptionKai = newShipGirl.ConsumptionKai;
                oldShipGirl.ConsumptionKai120 = newShipGirl.ConsumptionKai120;
                oldShipGirl.ConsumptionMax = newShipGirl.ConsumptionMax;
                oldShipGirl.Eq1EffInit = newShipGirl.Eq1EffInit;
                oldShipGirl.Eq1EffInitKai = newShipGirl.Eq1EffInitKai;
                oldShipGirl.Eq1EffInitMax = newShipGirl.Eq1EffInitMax;
                oldShipGirl.Eq1Type = newShipGirl.Eq1Type;
                oldShipGirl.Eq2EffInit = newShipGirl.Eq2EffInit;
                oldShipGirl.Eq2EffInitKai = newShipGirl.Eq2EffInitKai;
                oldShipGirl.Eq2EffInitMax = newShipGirl.Eq2EffInitMax;
                oldShipGirl.Eq2Type = newShipGirl.Eq2Type;
                oldShipGirl.Eq3EffInit = newShipGirl.Eq3EffInit;
                oldShipGirl.Eq3EffInitKai = newShipGirl.Eq3EffInitKai;
                oldShipGirl.Eq3EffInitMax = newShipGirl.Eq3EffInitMax;
                oldShipGirl.Eq3Type = newShipGirl.Eq3Type;
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
                oldShipGirl.Nationality = newShipGirl.Nationality;
                oldShipGirl.Oxygen120 = newShipGirl.Oxygen120;
                oldShipGirl.OxygenInitial = newShipGirl.OxygenInitial;
                oldShipGirl.OxygenKai = newShipGirl.OxygenKai;
                oldShipGirl.OxygenKai120 = newShipGirl.OxygenKai120;
                oldShipGirl.OxygenMax = newShipGirl.OxygenMax;
                oldShipGirl.Rarity = newShipGirl.Rarity;
                oldShipGirl.Reload120 = newShipGirl.Reload120;
                oldShipGirl.ReloadInitial = newShipGirl.ReloadInitial;
                oldShipGirl.ReloadKai = newShipGirl.ReloadKai;
                oldShipGirl.ReloadKai120 = newShipGirl.ReloadKai120;
                oldShipGirl.ReloadMax = newShipGirl.ReloadMax;
                oldShipGirl.Remodel = newShipGirl.Remodel;
                oldShipGirl.ShipGroup = newShipGirl.ShipGroup;
                // oldShipGirl.ShipID = newShipGirl.ShipID;
                oldShipGirl.Speed = newShipGirl.Speed;
                oldShipGirl.SpeedKai = newShipGirl.SpeedKai;
                oldShipGirl.SubtypeRetro = newShipGirl.SubtypeRetro;
                oldShipGirl.Torp120 = newShipGirl.Torp120;
                oldShipGirl.TorpInitial = newShipGirl.TorpInitial;
                oldShipGirl.TorpKai = newShipGirl.TorpKai;
                oldShipGirl.TorpKai120 = newShipGirl.TorpKai120;
                oldShipGirl.TorpMax = newShipGirl.TorpMax;
                oldShipGirl.Type = newShipGirl.Type;

                #endregion

                await SaveChangesAsync();
            }
        }

        /// <summary>
        /// Removes Ship Girl and her images
        /// </summary>
        /// <param name="shipGirl">Ship Girl to ERADICATE</param>
        /// <returns>TRUE - Success, FALSE - There is no such Ship Girl</returns>
        public async Task<bool> Remove(ShipGirl shipGirl)
        {
            if (ShipGirls.Find(shipGirl.ShipID) != null)
            {
                File.Delete(shipGirl.Image);
                File.Delete(shipGirl.ImageBanner);
                File.Delete(shipGirl.ImageBannerKai);
                File.Delete(shipGirl.ImageChibi);
                File.Delete(shipGirl.ImageChibiKai);
                File.Delete(shipGirl.ImageIcon);
                File.Delete(shipGirl.ImageIconKai);
                File.Delete(shipGirl.ImageKai);
                File.Delete(shipGirl.ImageShipyardIcon);
                File.Delete(shipGirl.ImageShipyardIconKai);

                ShipGirls.Remove(shipGirl);
                await SaveChangesAsync();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes Equipment and it image
        /// </summary>
        /// <param name="equipment">Equipment to ERADICATE</param>
        /// <returns>TRUE - Success, FALSE - There is no such Equipment</returns>
        public async Task<bool> Remove(Equipment equipment)
        {
            if (ShipGirlsEquipment.Find(equipment.EquipmentId) != null)
            {
                File.Delete(equipment.Image);

                ShipGirlsEquipment.Remove(equipment);
                await SaveChangesAsync();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes Skill and it image
        /// </summary>
        /// <param name="skill">Skill to ERADICATE</param>
        /// <returns>TRUE - Success, FALSE - There is no such Skill</returns>
        public async Task<bool> Remove(Skill skill)
        {
            if (Skills.Find(skill.SkillID) != null)
            {
                File.Delete(skill.Icon);

                Skills.Remove(skill);
                await SaveChangesAsync();

                return true;
            }

            return false;
        }
    }
}
