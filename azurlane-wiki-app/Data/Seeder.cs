using azurlane_wiki_app.Data.Downloaders;
using azurlane_wiki_app.Data.Tables;
using System.Threading.Tasks;

namespace azurlane_wiki_app.Data
{
    class Seeder
    {
        public async Task DownloadIcons()
        {
            IconDownloader iconDownloader = new IconDownloader();
            await iconDownloader.Download();

            using (CargoContext cargoContext = new CargoContext())
            {
                // Normal - fucked
                foreach (Rarity rarity in cargoContext.Rarities)
                {
                    rarity.Icon = rarity.Name + ".png";
                    await iconDownloader.Download(rarity.Icon);
                }
                
                // almost all fucked
                foreach (Nationality nationality in cargoContext.Nationalities)
                {
                    nationality.Icon = nationality.Name + ".png";
                    await iconDownloader.Download(nationality.Icon);
                }

                // all fucked
                foreach (ShipType shipType in cargoContext.ShipTypes)
                {
                    shipType.Icon = shipType.Abbreviation + ".png";
                    await iconDownloader.Download(shipType.Icon);
                }

                // all fucked
                foreach (SubtypeRetro subtypeRetro in cargoContext.SubtypeRetros)
                {
                    subtypeRetro.Icon = subtypeRetro.Abbreviation + ".png";
                    await iconDownloader.Download(subtypeRetro.Icon);
                }

                await cargoContext.SaveChangesAsync();
            }
        }
    }
}
