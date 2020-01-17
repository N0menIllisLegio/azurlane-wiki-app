using azurlane_wiki_app.Data.Tables;
using SQLite.CodeFirst;
using System.Data.Entity;

namespace azurlane_wiki_app.Data
{
    class CargoDbInitializer : SqliteCreateDatabaseIfNotExists<CargoContext>
    {
        public CargoDbInitializer(DbModelBuilder modelBuilder) : base(modelBuilder) { }

        protected override void Seed(CargoContext context)
        {
            InitializeWtgShipGirls(context);
        }

        private void InitializeWtgShipGirls(CargoContext context)
        {
            // Construction
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "Light" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "Heavy" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "Aviation" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "Limited" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "Exchange" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "Collection" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "Event" });

            // Drop Locations
            // 1
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "1-1" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "1-2" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "1-3" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "1-4" });
            // 2
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "2-1" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "2-2" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "2-3" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "2-4" });
            // 3
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "3-1" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "3-2" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "3-3" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "3-4" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "3-5" });
            // 4
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "4-1" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "4-2" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "4-3" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "4-4" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "4-5" });
            // 5
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "5-1" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "5-2" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "5-3" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "5-4" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "5-5" });
            // 6
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "6-1" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "6-2" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "6-3" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "6-4" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "6-5" });
            // 7
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "7-1" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "7-2" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "7-3" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "7-4" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "7-5" });
            // 8
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "8-1" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "8-2" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "8-3" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "8-4" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "8-5" });
            // 9
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "9-1" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "9-2" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "9-3" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "9-4" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "9-5" });
            // 10
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "10-1" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "10-2" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "10-3" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "10-4" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "10-5" });
            // 11
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "11-1" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "11-2" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "11-3" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "11-4" });
            // 12
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "12-1" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "12-2" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "12-3" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "12-4" });
            // 13
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "13-1" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "13-2" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "13-3" });
            context.Set<WhereToGetShipGirl>().Add(new WhereToGetShipGirl { Name = "13-4" });
        }
    }
}
