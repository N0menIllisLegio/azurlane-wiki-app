using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using azurlane_wiki_app.Data.Tables;

namespace azurlane_wiki_app.Data.Downloaders
{
    class IconDownloader : DataDownloader
    {
        private readonly string IconsFolderPath = ImagesFolderPath + "/Icons";
        private readonly List<string> IconsList = new List<string>
        {
            // Stat icons
            "Duration.png",
            "Armor.png",
            "Refill.png",
            "Luck.png",
            "Firepower.png",
            "Torpedo.png",
            "Evasion.png",
            "AntiAir.png",
            "Aviation.png",
            "Consumption.png",
            "Hit.png",
            "ASW.png"
        };

        public IconDownloader(int ThreadsCount = 0) : base(ThreadsCount)
        {
            if (!Directory.Exists(IconsFolderPath))
            {
                Directory.CreateDirectory(IconsFolderPath);
            }
        }

        /// <summary>
        /// Downloads stat icons
        /// </summary>
        public override async Task Download()
        {
            Status = Statuses.InProgress;

            using (CargoContext cargoContext = new CargoContext())
            {
                foreach (string icon in IconsList)
                {
                    downloadBlock.Post(icon);
                    cargoContext.Icons.Add(new Icon
                    {
                        Name = icon.Replace(".png", ""), 
                        FileName = icon
                    });
                }

                downloadBlock.Complete();
                cargoContext.SaveChanges();
            }

            downloadBlock.Completion.Wait();
            Status = Statuses.DownloadComplete;
        }

        public override async Task Download(string id)
        {
            Status = Statuses.InProgress;
            string imagePath = GetImageFolder(id) + "/" + id;

            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }

            await DownloadImage(id);

            Status = Statuses.DownloadComplete;
        }

        public override string GetImageFolder(string imageName) => IconsFolderPath;
    }
}
