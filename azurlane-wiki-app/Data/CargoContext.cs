using System.Data.Entity;
using SQLite.CodeFirst;

namespace azurlane_wiki_app
{
    class CargoContext : DbContext
    {
        private static CargoContext _instance;
        public static CargoContext Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CargoContext();
                }

                return _instance;
            }
        }

        public DbSet<ShipGirl> ShipGirls { get; set; }

        public CargoContext() : base("CargoConnection") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var sqliteInitializer = new SqliteCreateDatabaseIfNotExists<CargoContext>(modelBuilder);
            Database.SetInitializer(sqliteInitializer);
        }
    }
}
