using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace azurlane_wiki_app.Data
{
    enum Statuses
    {
        Pending,
        DownloadComplete,
        ErrorInDeserialization,
        InProgress
    }

    abstract class DataDownloader : INotifyPropertyChanged
    {
        protected const string ImagesFolderPath = "./Images";
        protected const string WikiApiEndpoint = "https://azurlane.koumakan.jp/w/api.php";
        private const string ImageInfoApiEndpoint =
            "https://azurlane.koumakan.jp/w/api.php?action=query&format=json&list=allimages&ailimit=1&aifrom=";

        public abstract Task Download();

        /// <summary>
        /// Gets path to folder for image. Depends on image name. (Image containing ShipyardIcon in name gets path to ShipyardIcons folder).
        /// </summary>
        /// <param name="imageName">Image name (example: 22Chibi.png)</param>
        /// <returns></returns>
        public abstract string GetImageFolder(string imageName);

        /// <summary>
        /// Gets all data from specified table.
        /// </summary>
        /// <param name="table">Table to get data from</param>
        /// <param name="fields">Fields from table</param>
        /// <returns>Json array with data</returns>
        protected async Task<string> GetData(string table, string fields)
        {
            int offset = 0;

            string result = "[";
            string responseJson;

            do
            {
                string requestUrl = WikiApiEndpoint + "?action=cargoquery&tables=" + table
                                    + "&format=json&fields=" + fields + "&limit=500&offset=" + offset;
                responseJson = "";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();

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

                responseJson = responseJson.Remove(0, 15);           // remove {"cargoquery":[
                responseJson = responseJson.Remove(responseJson.Length - 2);        // remove ]}

                result += responseJson + ",";
                offset += 500;
            } while (responseJson.Length != 0);

            return result.Remove(result.Length - 2) + ']'; // remove last , and add ]
        }

        /// <summary>
        /// Gets all data from joined tables.
        /// </summary>
        /// <param name="tables">Tables to get data from (shipDrops,ship_skills,equipment,ships)</param>
        /// <param name="fields">Fields from tables (shipDrops.Name, ships.ShipID etc)</param>
        /// <param name="joinOn">Fields to join on (ships._pageName=ship_skills._pageName)</param>
        /// <returns>Json array with data</returns>
        protected async Task<string> GetData(string tables, string fields, string joinOn)
        {
            int offset = 0;
            int limit = 500;

            string result = "[";
            string responseJson;

            do
            {
                string requestUrl = WikiApiEndpoint + "?action=cargoquery&tables=" + tables
                                    + "&format=json&fields=" + fields + "&limit=" + limit + "&offset=" + offset + "&join_on=" + joinOn;
                responseJson = "";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();

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

                responseJson = responseJson.Remove(0, 15);           // remove {"cargoquery":[
                responseJson = responseJson.Remove(responseJson.Length - 2);        // remove ]}

                result += responseJson + ",";
                offset += 500;
            } while (responseJson.Length != 0);

            return result.Remove(result.Length - 2) + ']'; // remove last , and add ]
        }

        #region Notify

        private int _currentImageCount;
        private int _totalImageCount;
        private Statuses _statuses;

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

        public Statuses Status
        {
            get => _statuses;
            set
            {
                _statuses = value;
                OnPropertyChanged(nameof(Status));
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
            Status = Statuses.Pending;

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
