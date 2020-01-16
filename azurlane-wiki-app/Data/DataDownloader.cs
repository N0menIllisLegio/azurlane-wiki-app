using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace azurlane_wiki_app.Data
{
    abstract class DataDownloader : INotifyPropertyChanged
    {
        protected const string ImagesFolderPath = "./Images";
        private const string WikiApiEndpoint = "https://azurlane.koumakan.jp/w/api.php";
        private const string ImageInfoApiEndpoint =
            "https://azurlane.koumakan.jp/w/api.php?action=query&format=json&list=allimages&ailimit=1&aifrom=";

        public abstract void Download();

        /// <summary>
        /// Gets path to folder for image. Depends on image name. (Image containing ShipyardIcon in name gets path to ShipyardIcons folder).
        /// </summary>
        /// <param name="imageName">Image name (example: 22Chibi.png)</param>
        /// <returns></returns>
        public abstract string GetImageFolder(string imageName);

        protected async Task<string> GetData(string table, string fields)
        {
            //TODO: Remove limit (Default = 50)
            string requestUrl = WikiApiEndpoint + "?action=cargoquery&tables=" + table 
                                + "&format=json&fields=" + fields + "&limit=500";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();

            string responseJson = "";

            using (Stream stream = response?.GetResponseStream())
            {
                if (stream != null)
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        responseJson = reader.ReadToEnd();
                    }
                }
            }

            responseJson = responseJson.Remove(0, 14);   // remove {"cargoquery":
            return responseJson.Remove(responseJson.Length - 1);        // remove }
        }

        #region Notify

        private int _currentImageCount;
        private int _totalImageCount;

        public int TotalImageCount
        {
            get => _totalImageCount;
            set
            {
                _totalImageCount = value;
                OnPropertyChanged(nameof(TotalImageCount));
            }
        }

        public int CurrentImageCount
        {
            get => _currentImageCount;
            set
            {
                _currentImageCount = value;
                OnPropertyChanged(nameof(CurrentImageCount));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        protected DataDownloader()
        {
            if (!Directory.Exists(ImagesFolderPath))
            {
                Directory.CreateDirectory(ImagesFolderPath);
            }
        }

        /// <summary>
        /// Download image by its name.
        /// </summary>
        /// <param name="imageName">Image name (example: 22Chibi.png)</param>
        public async Task DownloadImage(string imageName)
        {
            string imageUrl;

            try
            {
                imageUrl = await GetImageInfo(imageName);
            }
            catch
            {
                //TODO: Add error display

                imageUrl = "";
            }

            if (imageName != "" && imageUrl != "")
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                                       SecurityProtocolType.Tls11 |
                                                       SecurityProtocolType.Tls12;

                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(imageUrl);
                request.Timeout = 14400000;
                request.KeepAlive = true;

                using (HttpWebResponse response = (HttpWebResponse) await request.GetResponseAsync())
                {
                    try
                    {
                        Stream stream = response.GetResponseStream();

                        if (stream != null)
                        {
                            using (BinaryReader binaryReader = new BinaryReader(stream))
                            {
                                using (MemoryStream memoryStream = new MemoryStream())
                                {
                                    byte[] buffer = binaryReader.ReadBytes(1024);

                                    while (buffer.Length > 0)
                                    {
                                        memoryStream.Write(buffer, 0, buffer.Length);
                                        buffer = binaryReader.ReadBytes(1024);
                                    }

                                    byte[] fileBuffer = new byte[(int) memoryStream.Length];
                                    memoryStream.Position = 0;
                                    memoryStream.Read(fileBuffer, 0, fileBuffer.Length);

                                    string folderName = GetImageFolder(imageName);

                                    using (FileStream fileStream =
                                        new FileStream(folderName + "/" + imageName, FileMode.Create))
                                    {
                                        fileStream.Write(fileBuffer, 0, fileBuffer.Length);
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                        //TODO: Add error display
                    }
                }
            }
            CurrentImageCount++;
        }

        /// <summary>
        /// Gets image URL from image name.
        /// </summary>
        /// <param name="imageName">Image name (example: 22Chibi.png)</param>
        /// <returns></returns>
        private async Task<string> GetImageInfo(string imageName)
        {
            string result = "";

            string dirName = GetImageFolder(imageName);

            if (imageName != "" && !File.Exists(dirName + "/" + imageName))
            {
                string requestUrl = ImageInfoApiEndpoint + imageName;
                string responseJson = "";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);

                using (HttpWebResponse response = (HttpWebResponse) await request.GetResponseAsync())
                {
                    using (Stream stream = response?.GetResponseStream())
                    {
                        if (stream != null)
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                responseJson = reader.ReadToEnd();
                            }
                        }
                    }
                }

                Regex name = new Regex("\"url\":\".*?\"", RegexOptions.Compiled);
                MatchCollection matches = name.Matches(responseJson);

                if (matches.Count > 0)
                {
                    requestUrl = matches[0].Value.Remove(0, 7); // removing "url":" 
                    requestUrl = requestUrl.Remove(requestUrl.Length - 1); // removing last "

                    result = requestUrl;
                }
            }

            return result;
        }
    }
}
