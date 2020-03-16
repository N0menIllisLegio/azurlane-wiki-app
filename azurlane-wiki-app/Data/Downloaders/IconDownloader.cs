using azurlane_wiki_app.Data.Tables;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace azurlane_wiki_app.Data.Downloaders
{
    class IconDownloader : DataDownloader
    {
        private static readonly string IconsFolderPath = ImagesFolderPath + "/Icons";

        private TransformBlock<string, string> urlGetterBlock;

        private ActionBlock<string> urlDownloaderBlock;

        public IconDownloader(int threadsCount = 0) : base(threadsCount)
        {
            if (!Directory.Exists(IconsFolderPath))
            {
                Directory.CreateDirectory(IconsFolderPath);
            }

            async Task<string> UrlGetterFunction(string iconName)
            {
                string iconTag = await GetIconsTag(iconName);

                if (iconTag != "")
                {
                    string iconUrl = GetIconsUrl(iconTag);

                    if (iconUrl != null)
                    {
                        using (CargoContext cargoContext = new CargoContext())
                        {
                            Icon icon = new Icon
                            {
                                Name = iconName.Replace("Template:", ""),
                                FileName = iconUrl.Split('/').Last()
                            };

                            if (await cargoContext.Icons.FindAsync(icon.Name) == null)
                            {
                                cargoContext.Icons.Add(icon);
                                await cargoContext.SaveChangesAsync();
                            }
                        }

                        return iconUrl;
                    }
                }

                return null;
            }

            async Task UrlDownloaderFunction(string iconUrl) => 
                await DownloadImageByUrl(iconUrl, iconUrl?.Split('/').Last());

            urlGetterBlock = threadsCount <= 0
                ? new TransformBlock<string, string>(UrlGetterFunction)
                : new TransformBlock<string, string>(UrlGetterFunction, 
                    new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = threadsCount });

            urlDownloaderBlock = threadsCount <= 0
                ? new ActionBlock<string>(UrlDownloaderFunction)
                : new ActionBlock<string>(UrlDownloaderFunction, 
                    new ExecutionDataflowBlockOptions { MaxDegreeOfParallelism = threadsCount });

            urlGetterBlock.LinkTo(urlDownloaderBlock);
            urlGetterBlock.Completion.ContinueWith(delegate { urlDownloaderBlock.Complete(); });
        }

        /// <summary>
        /// Download all icons from https://azurlane.koumakan.jp/Category:Icon_templates
        /// and save their names and path in DB.
        /// </summary>
        public override async Task Download()
        {
            Status = Statuses.InProgress;
            HashSet<string> iconsNames = await GetIconsNames();

            foreach (string iconsName in iconsNames)
            {
                urlGetterBlock.Post(iconsName);
            }

            urlGetterBlock.Complete();
            urlDownloaderBlock.Completion.Wait();
            Status = Statuses.DownloadComplete;
        }

        /// <summary>
        /// Download image by name.
        /// https://azurlane.koumakan.jp/Template: + id
        /// </summary>
        /// <param name="id">Image name (without "Template:" and without extension)</param>
        /// <returns></returns>
        public override async Task Download(string id)
        {
            Status = Statuses.InProgress;
            string imagePath = GetImageFolder(id) + "/" + id;

            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }

            string tag = await GetIconsTag("Template:" + id);
            string url = GetIconsUrl(tag);

            if (!string.IsNullOrEmpty(url))
            {
                using (CargoContext cargoContext = new CargoContext())
                {
                    Icon icon = new Icon
                    {
                        Name = id,
                        FileName = (await DownloadImageByUrl(url, id + ".png")).Split('/').Last()
                    };

                    if (await cargoContext.Icons.FindAsync(icon.Name) == null)
                    {
                        cargoContext.Icons.Add(icon);
                        await cargoContext.SaveChangesAsync();
                    }
                }
            }

            Status = Statuses.DownloadComplete;
        }

        /// <summary>
        /// Get path to image folder that stores icons.
        /// </summary>
        /// <param name="imageName">Name of icon for saving</param>
        /// <returns>Path</returns>
        public override string GetImageFolder(string imageName) => IconsFolderPath;

        /// <summary>
        /// Get all icons names from https://azurlane.koumakan.jp/Category:Icon_templates
        /// </summary>
        /// <returns>Icon names in format: Template:ICON</returns>
        private async Task<HashSet<string>> GetIconsNames()
        {
            HashSet<string> iconPages = new HashSet<string>();
            string requestUrl = "https://azurlane.koumakan.jp/Category:Icon_templates";
            string responseJson = "";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();

            using (Stream stream = response?.GetResponseStream())
            {
                if (stream != null)
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        responseJson = await reader.ReadToEndAsync();
                    }
                }
            }

            Regex regex = new Regex(@"""Template:(\w|\-|\s)+""");
            MatchCollection matches = regex.Matches(responseJson);
            
            foreach (Match match in matches)
            {
                iconPages.Add(match.Value.Replace("\"", ""));
            }

            return iconPages;
        }

        /// <summary>
        /// Get img tag for icon.
        /// Load page with icon (https://azurlane.koumakan.jp/ + iconName), parse it, get img tag.
        /// </summary>
        /// <param name="iconName">Icon name in format: Template:ICON</param>
        /// <returns>img tag</returns>
        private async Task<string> GetIconsTag(string iconName)
        {
            string responseJson = "";
            string pageUrl = "https://azurlane.koumakan.jp/";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(pageUrl + iconName.Replace(' ', '_'));
            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();

            using (Stream stream = response?.GetResponseStream())
            {
                if (stream != null)
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        responseJson = await reader.ReadToEndAsync();
                    }
                }
            }

            Regex regex = new Regex(@"<img alt.+?>");
            Match match = regex.Match(responseJson);
            return match.Success ? match.Value : ""; // iconName
        }

        /// <summary>
        /// Get Icon URL from tag (src)
        /// </summary>
        /// <param name="iconTag">img tag</param>
        /// <returns>URL to image</returns>
        private string GetIconsUrl(string iconTag)
        {
            string iconUrl = "https://azurlane.koumakan.jp/w/images/";

            Regex getSrc = new Regex(@"src="".+?""");
            Match match = getSrc.Match(iconTag);

            if (match.Success)
            {
                string temp = match.Value.Replace("src=\"/w/images/thumb/", "");
                temp = temp.Remove(temp.LastIndexOf('/'));

                iconUrl += temp;
            }
            else
            {
                return null;
            }

            return iconUrl;
        }

        /// <summary>
        /// Create relationships with icons
        /// </summary>
        public void ConnectIcons()
        {
            using (CargoContext cargoContext = new CargoContext())
            {
                foreach (SubtypeRetro subtypeRetro in cargoContext.SubtypeRetros)
                {
                    subtypeRetro.FK_Icon = cargoContext.Icons.Find(subtypeRetro.Abbreviation);
                }

                foreach (ShipType shipType in cargoContext.ShipTypes)
                {
                    shipType.FK_Icon = cargoContext.Icons.Find(shipType.Abbreviation);
                }

                foreach (Rarity rarity in cargoContext.Rarities)
                {
                    rarity.FK_Icon = cargoContext.Icons.Find(rarity.Name);
                }

                foreach (Nationality nationality in cargoContext.Nationalities)
                {
                    nationality.FK_Icon = cargoContext.Icons.Find(nationality.Name);
                }

                cargoContext.SaveChanges();
            }
        }
    }
}