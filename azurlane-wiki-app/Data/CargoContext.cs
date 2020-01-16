using System.Data.Entity;
using azurlane_wiki_app.Data;
using SQLite.CodeFirst;

namespace azurlane_wiki_app
{
    class CargoContext : DbContext
    {
        public DbSet<ShipGirl> ShipGirls { get; set; }
        public DbSet<Equipment> ShipGirlsEquipment { get; set; }
        public DbSet<Skill> Skills { get; set; }

        public CargoContext() : base("CargoConnection") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var sqliteInitializer = new SqliteCreateDatabaseIfNotExists<CargoContext>(modelBuilder);
            Database.SetInitializer(sqliteInitializer);
        }
    }
}
