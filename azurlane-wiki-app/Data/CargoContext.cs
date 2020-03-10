using azurlane_wiki_app.Data;
using azurlane_wiki_app.Data.Tables;
using System.Data.Entity;
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
            var sqliteInitializer = new CargoDbInitializer(modelBuilder);
            Database.SetInitializer(sqliteInitializer);
        }

        public async Task CreateRelationshipGirlDrop(WhereToGetShipGirl wtg, ShipGirl shipGirl, string note)
        {
            ShipGirlWhereToGetShipGirl mtm = new ShipGirlWhereToGetShipGirl
            {
                FK_ShipGirl = shipGirl, 
                FK_WhereToGetShipGirl = wtg, 
                Note = note
            };

            if (ShipGirlWhereToGetShipGirl.Count(e => e.FK_ShipGirl.ShipID == mtm.FK_ShipGirl.ShipID 
                                                      && e.FK_WhereToGetShipGirl.Name == mtm.FK_WhereToGetShipGirl.Name) == 0)
            {
                ShipGirlWhereToGetShipGirl.Add(mtm);
                await SaveChangesAsync();
            }
        }

        public bool Remove(ShipGirl shipGirl)
        {
            if (ShipGirls.Find(shipGirl.ShipID) != null)
            {
                // File.Delete();
            }

            return false;
        }
    }
}
